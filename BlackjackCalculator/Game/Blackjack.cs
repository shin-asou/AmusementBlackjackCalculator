using BlackjackCalculator.Factory;
using BlackjackCalculator.Item;
using BlackjackCalculator.Strategy;

namespace BlackjackCalculator.Game
{
    public enum HandAction
    {
        Hit,
        Stand,
        DoubleDown,
        Split,
        Surrender,
        HitOrDoubleDown,
        HitOrSurrender
    }
    public enum GamePreAction
    {
        No,
        Insurance,
        EvenMoney,
        Surrender,
    }
    public enum HandResult
    {
        Value, // 21以下の数値かつ特殊役ではない
        Burst,
        Blackjack,
        SixUnder,
        SevenUnder,
        EightUnder,
        Straight,
        ThreeSeven,
        Ace2Six
    }
    public enum GameResult
    {
        No, // Preaction用まだゲーム中の状態
        WinByBlackjack,
        Win,
        Lose,
        LoseBySurrender,
        Push,
        WinBySixUnder,
        WinBySevenUnder,
        WinByEightUnder,
        WinByStraight,
        WinByThreeSeven,
        WinByAce2Six
    }
    public enum SurrenderType
    {
        No,
        Late,
        Early,
    }
    public enum DoubledownType
    {
        AnyTwoCard,
        Nine2Eleven,
        Ten2Eleven,
    }

    public class Blackjack
    {
        // Flow
        public void Flow()
        {
            var record = CreateBlackjackRecord(10);
            bool isLastGame = false;
            bool canGame = true;
            // shooterからカードを引ける限りゲームを続行
            while (canGame)
            {
                GameSequence(record.Rule, record.Dealer, record.Players, record.Shooter);
                if (isLastGame) canGame = false;
                isLastGame = true; // 今は次のゲーム用の処理がないので強制的に抜けるようにしておく
                // cutCard判定 ゲーム終了時に次がカットカードだったらもしくはすでに出てる場合は次のゲームを終了とする
                if (!isLastGame) isLastGame = record.Shooter.IsEndGame;
            }
        }

        // TODO: ↓
        // GameSequence
        // FirstDeal カードを配る (ついでにプレイヤーとディーラーのストラテジーを作る
        // DealerPreAction ディーラーBlackjackチェック + プレイヤー側のプレアクション(イーブンマネー等)
        // PlayersAction プレイヤースプリット処理
        // PlayerAction プレイヤーごとのアクション
        // DealerAction ディーラーアクション
        // ---- 結果に応じて配当
        public static List<PlayerStrategy> GameSequence(RuleSet rule, DealerStrategy dealer, List<PlayerStrategy> players, Shooter shooter)
        {
            // DealerBlackjack + Early Surrender Check
            DealerPreAction(rule, dealer, players);
            var result = PlayersAction(rule, dealer, players, shooter);
            DealerAction(dealer, shooter);
            // 現在は暫定的にプレイヤーリストを返すが最終的には配当リストを返す
            return result;
        }

        public static void DealerAction(DealerStrategy dealer, Shooter shooter)
        {
            var actionResult = HandAction.Hit;
            while (actionResult != HandAction.Stand)
            {
                actionResult = dealer.Action();
                if (actionResult == HandAction.Hit) dealer.Hit(shooter.Pull());
            }
        }

        public static List<PlayerStrategy> PlayersAction(RuleSet rule, DealerStrategy dealer, List<PlayerStrategy> players, Shooter shooter)
        {
            var result = new List<PlayerStrategy>();
            while (players.Count > 0)
            {
                var player = players.First();
                if (player.IsEndByPreAction() || dealer.IsBlackjack)
                {
                    result.Add(player);
                }
                else
                {
                    result = PlayerActionMain(rule, dealer, shooter, result, player);
                }
                players.RemoveAt(0);
            }

            return result;
        }

        private static List<PlayerStrategy> PlayerActionMain(RuleSet rule, DealerStrategy dealer, Shooter shooter, List<PlayerStrategy> result, PlayerStrategy player)
        {
            var action = PlayerAction(rule, player, dealer.UpCard, shooter);
            if (action == HandAction.Split)
            {
                List<PlayerStrategy> splitTree = SplitTreeAction(rule, dealer, shooter, player);
                result.AddRange(splitTree.Where(usePlayer => !usePlayer.IsNull));
            }
            else
            {
                result.Add(player);
            }
            return result;
        }

        private static List<PlayerStrategy> SplitTreeAction(RuleSet rule, DealerStrategy dealer, Shooter shooter, PlayerStrategy player)
        {
            var result = CreateSplitTree(rule, player, shooter);
            for (var i = 0; i < result.Count; i++)
            {
                var strategy = result[i];
                if (strategy.IsNull) continue;
                // blankCardがあったら作りなおす
                if (strategy.IsBlank)
                {
                    result[i] = CreatePlayerStrategy(strategy.FirstCard, shooter.Pull(), rule, i + 1);
                    strategy = result[i];
                }
                PlayerActionBySplit(rule, dealer.UpCard, shooter, strategy, i, result);
            }
            return result;
        }

        private static List<PlayerStrategy> CreateSplitTree(RuleSet rule, PlayerStrategy player, Shooter shooter)
        {
            var result = new List<PlayerStrategy>();
            for (var i = 0; i < rule.MaxSplit; i++)
            {
                result.Add(CreateNullPlayerStrategy(player.FirstCard, i + 1));
            }
            result[0] = CreatePlayerStrategy(player.FirstCard, shooter.Pull(), rule, 1);
            result[1] = CreatePlayerStrategy(player.SecondCard, BlankCard.Build(), rule, 2);
            return result;
        }

        private static void PlayerActionBySplit(RuleSet rule, Card upCard, Shooter shooter, PlayerStrategy player, int treeIndex, List<PlayerStrategy> splitTree)
        {
            var action = PlayerAction(rule, player, upCard, shooter);
            if (splitTree.All(strategy => !strategy.IsNull)) return;
            if (action == HandAction.Split)
            {
                var tempSecondCard = player.SecondCard;
                splitTree[treeIndex] = CreatePlayerStrategy(player.FirstCard, shooter.Pull(), rule, treeIndex);
                player = splitTree[treeIndex];
                // Splitされた次のboxのためにnullStrategyではなくBlankCardを持つデータを作っておく
                int nextIndex = splitTree.FindIndex(split => split.IsNull);
                splitTree[nextIndex] = CreatePlayerStrategy(tempSecondCard, BlankCard.Build(), rule, nextIndex + 1);
                if (splitTree.All(strategy => !strategy.IsNull)) splitTree.ForEach(strategy => strategy.IsMaxSplitTree = true);
                // ここは次のカードではなく今のカードのアクションをしなければいけない
                PlayerActionBySplit(rule, upCard, shooter, player, treeIndex, splitTree);
            }
        }

        public static void DealerPreAction(RuleSet rule, DealerStrategy dealer, List<PlayerStrategy> players)
        {
            var judge = new Judgment(rule);
            judge.PreActionPlayers(dealer, players);
        }

        // 再帰以外で返るのはHandAction.Stand or HandAction.Surrender or HandAction.Split
        public static HandAction PlayerAction(RuleSet rule, PlayerStrategy player, Card upCard, Shooter shooter)
        {
            HandAction result = player.Action(upCard);
            if (player.CanNotHit() || result == HandAction.Stand) return HandAction.Stand;
            if (IsPlayerActionSurrender(rule, player, result)) return HandAction.Surrender;
            if (result == HandAction.Split) return HandAction.Split;
            if (IsPlayerActionDoubleDown(player, result))
            {
                player.DoubleDown(shooter.Pull());
                return HandAction.Stand;
            }
            // TODO: Hit SurrenderType == No でもHitOrSurrenderしかないのでここで強制的に引く
            player.Hit(shooter.Pull());
            return PlayerAction(rule, player, upCard, shooter);
        }

        // 先頭がdealerのハンド、残りがプレーヤーのハンドのstrategyリストを返す
        public static List<AbstractStrategy> FirstDeal(RuleSet rule, Shooter shooter, int boxCount)
        {
            var result = new List<AbstractStrategy>();
            var dealHand = FirstDealCardPull(shooter, boxCount);
            var halfCount = dealHand.Count / 2;
            // もしboxCount = 2の場合
            // 0, 1, 2*,3,4,5*
            // もしboxCount = 3の場合
            // 0, 1, 2,3*,4,5,6,7*
            result.Add(CreateDealerStrategy(dealHand[halfCount - 1], dealHand[^1], rule: rule));
            for (int i = 0; i < boxCount; i++)
            {
                result.Add(CreatePlayerStrategy(dealHand[i], dealHand[i + halfCount], rule: rule));
            }
            return result;
        }

        // box数のカード+ディーラーのカードを引いてリストで返す
        public static List<Card> FirstDealCardPull(Shooter shooter, int boxCount)
        {
            var result = new List<Card>();
            // dealer用に1を足しておく
            var boxCardCount = (1 + boxCount) * 2;
            for (int i = 0; i < boxCardCount; i++)
            {
                result.Add(shooter.Pull());
            }
            return result;
        }

        private static DealerStrategy PullDealerStrategy(List<AbstractStrategy> strategies)
        {
            var dealer = (DealerStrategy)strategies.First();
            strategies.RemoveAt(0);
            return dealer;
        }
        private static List<PlayerStrategy> CastListAbstractStrategy2PlayerStrategy(List<AbstractStrategy> strategies)
        {
            var result = new List<PlayerStrategy>();
            foreach (PlayerStrategy strategy in strategies.Cast<PlayerStrategy>())
            {
                result.Add(strategy);
            }
            return result;
        }

        private static bool IsPlayerActionSurrender(RuleSet rule, PlayerStrategy player, HandAction result)
        {
            return result == HandAction.HitOrSurrender && rule.Surrender == SurrenderType.Late && player.CanSurrender;
        }

        private static bool IsPlayerActionDoubleDown(PlayerStrategy player, HandAction result)
        {
            return (result == HandAction.HitOrDoubleDown || result == HandAction.DoubleDown) && player.CanDoubleDown();
        }

        // 任意のfactoryに差し替える仕組みをいれることによって各アクションの多様性を担保できる
        protected static DealerStrategy CreateDealerStrategy(Card firstCard, Card secondCard, RuleSet rule) => StrategyFactory.BuildDealer(firstCard, secondCard, rule);
        // player strategy Create
        protected static PlayerStrategy CreatePlayerStrategy(Card firstCard, Card secondCard, RuleSet rule, int splitCount = 0) => StrategyFactory.BuildBasic(firstCard, secondCard, rule, splitCount: splitCount);
        private static PlayerStrategy CreateNullPlayerStrategy(Card firstCard, int splitCount) => StrategyFactory.BuildNullPlayer(firstCard, splitCount);

        public static BlackjackRecord CreateBlackjackRecord(int boxCount)
        {
            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildShooter(rule.DeckCount, rule.EndDeckCount);
            var boxes = FirstDeal(rule, shooter, boxCount);
            var dealer = PullDealerStrategy(boxes);
            var players = CastListAbstractStrategy2PlayerStrategy(boxes);
            return new BlackjackRecord(
                rule,
                shooter,
                dealer,
                players
                );
        }
    }
    // テストのために必要なものをまとめるクラスを作っておく将来的には消えるかもしれない
    public readonly record struct BlackjackRecord(RuleSet Rule, Shooter Shooter, DealerStrategy Dealer, List<PlayerStrategy> Players) { }
}

