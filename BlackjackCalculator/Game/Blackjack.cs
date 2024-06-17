using BlackjackCalculator.Factory;
using BlackjackCalculator.Item;
using BlackjackCalculator.Strategy;
using System.Diagnostics;

namespace BlackjackCalculator.Game
{
    public enum HandAction
    {
        Hit,
        Stand,
        DoubleDown,
        Split,
        Surrender,
        StandOrDoubleDown,
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

    public class Blackjack(RuleSet rule)
    {
        public RuleSet Rule { get; } = rule;
        // Flow
        public void Flow()
        {
            var all = new List<List<BoxPayout>>();
            for (int j = 0; j < 1500; j++)
            {
                var result = new List<BoxPayout>();
                for (int i = 0; i < 200; i++)
                {
                    var shooter = ShooterFactory.BuildShooter(Rule.DeckCount, Rule.EndDeckCount);
                    result.AddRange(ShooterSequence(Rule, shooter));
                }
                all.Add(result);
            }

            all.Sort((a, b) =>
            {
                return (int)a.Sum(p => p.Payout()) - (int)b.Sum(p => p.Payout());
            });
            int count = 0;
            foreach (var item in all)
            {
                Debug.WriteLine("Index:" + ++count);
                Debug.WriteLine("件数:" + item.Count);
                Debug.WriteLine("勝利数:" + item.Count(p => p.IsWin));
                Debug.WriteLine("Player Blackjack:" + item.Count(p => p.IsPlayerBlakjack));
                Debug.WriteLine("Dealer Blackjack:" + item.Count(p => p.IsDealerBlackjack));
                Debug.WriteLine("Dealer & Player Blackjack:" + item.Count(p => p.IsPushBlackjack));
                Debug.WriteLine("Player Straight:" + item.Count(p => p.Result == GameResult.WinByStraight));
                Debug.WriteLine("Player ThreeSeven:" + item.Count(p => p.Result == GameResult.WinByThreeSeven));

                Debug.WriteLine("Player 6Under:" + item.Count(p => p.Result == GameResult.WinBySixUnder));
                Debug.WriteLine("Player 7Under:" + item.Count(p => p.Result == GameResult.WinBySevenUnder));
                Debug.WriteLine("Player 8Under:" + item.Count(p => p.Result == GameResult.WinByEightUnder));

                Debug.WriteLine("Player Ace to Six:" + item.Count(p => p.Result == GameResult.WinByAce2Six));
                var total = item.Sum(p => p.Payout());
                Debug.WriteLine("総収支:" + total);
                var ev = total / item.Count;
                Debug.WriteLine("1ゲームあたりの収支期待値:" + Math.Round(ev, 5));
                Debug.WriteLine("期待値:" + Math.Round(100 + ((ev / 600m) * 100), 5));
                decimal burstCount = item.Count(p => p.Dealer.Result() == HandResult.Burst);
                Debug.WriteLine("Dealer Burst:" + burstCount);
                Debug.WriteLine("Burst %:" + Math.Round((burstCount / item.Count) * 100, 5));
                Debug.WriteLine("");
            }


            Debug.WriteLine("件数:" + all.Sum(r => r.Count));
            Debug.WriteLine("勝利数:" + all.Sum(r => r.Count(p => p.IsWin)));
            Debug.WriteLine("Player Blackjack:" + all.Sum(r => r.Count(p => p.IsPlayerBlakjack)));
            Debug.WriteLine("Dealer Blackjack:" + all.Sum(r => r.Count(p => p.IsDealerBlackjack)));
            Debug.WriteLine("Dealer & Player Blackjack:" + all.Sum(r => r.Count(p => p.IsPushBlackjack)));
            Debug.WriteLine("Player Straight:" + all.Sum(r => r.Count(p => p.Result == GameResult.WinByStraight)));
            Debug.WriteLine("Player ThreeSeven:" + all.Sum(r => r.Count(p => p.Result == GameResult.WinByThreeSeven)));

            Debug.WriteLine("Player 6Under:" + all.Sum(r => r.Count(p => p.Result == GameResult.WinBySixUnder)));
            Debug.WriteLine("Player 7Under:" + all.Sum(r => r.Count(p => p.Result == GameResult.WinBySevenUnder)));
            Debug.WriteLine("Player 8Under:" + all.Sum(r => r.Count(p => p.Result == GameResult.WinByEightUnder)));

            Debug.WriteLine("Player Ace to Six:" + all.Sum(r => r.Count(p => p.Result == GameResult.WinByAce2Six)));
            var alltotal = all.Sum(r => r.Sum(p => p.Payout()));
            Debug.WriteLine("総収支:" + alltotal);
            var allev = alltotal / all.Sum(r => r.Count);
            Debug.WriteLine("1ゲームあたりの収支期待値:" + Math.Round(allev, 5));
            Debug.WriteLine("期待値:" + Math.Round(100 + ((allev / 600m) * 100), 5));
            decimal allburstCount = all.Sum(r => r.Count(p => p.Dealer.Result() == HandResult.Burst));
            Debug.WriteLine("Dealer Burst:" + allburstCount);
            Debug.WriteLine("Burst %:" + Math.Round((allburstCount / all.Sum(r => r.Count)) * 100, 5));
            Debug.WriteLine("");
            decimal allplayerBurstCount = all.Sum(r => r.Count(p => p.Player.Result() == HandResult.Burst));
            Debug.WriteLine("Player Burst:" + allplayerBurstCount);
            Debug.WriteLine("Burst %:" + Math.Round((allplayerBurstCount / all.Sum(r => r.Count)) * 100, 5));
            Debug.WriteLine("");

            Debug.WriteLine("マイナス収支の回数:" + all.Count(r => r.Sum(p => p.Payout()) < 0));
            Debug.WriteLine("1ループあたりの平均収支:" + Math.Round(all.Sum(r => r.Sum(p => p.Payout())) / all.Count));

        }

        private List<BoxPayout> ShooterSequence(RuleSet rule, Shooter shooter)
        {
            bool isLastGame = false;
            bool canGame = true;
            var result = new List<BoxPayout>();
            // shooterからカードを引ける限りゲームを続行
            while (canGame)
            {
                var boxes = FirstDeal(shooter, 1);
                var dealer = PullDealerStrategy(boxes);
                var players = CastListAbstractStrategy2PlayerStrategy(boxes);
                result.AddRange(GameSequence(rule, dealer, players, shooter));
                if (isLastGame) return result;
                // cutCard判定 ゲーム終了時に次がカットカードだったらもしくはすでに出てる場合は次のゲームを終了とする
                if (!isLastGame) isLastGame = shooter.IsEndGame;
            }
            return result;
        }

        public List<BoxPayout> GameSequence(RuleSet rule, DealerStrategy dealer, List<PlayerStrategy> players, Shooter shooter)
        {
            // DealerBlackjack + Early Surrender Check
            DealerPreAction(dealer, players);
            var playerActionList = PlayersAction(dealer, players, shooter);
            DealerAction(dealer, shooter);
            var result = Payout(rule, dealer, playerActionList);

            // 現在は暫定的にプレイヤーリストを返すが最終的には配当リストを返す
            return result;
        }
        public static List<BoxPayout> Payout(RuleSet rule, DealerStrategy dealer, List<PlayerStrategy> players)
        {
            var result = new List<BoxPayout>();
            var judgement = new Judgment(rule);
            foreach (var player in players)
            {
                result.Add(judgement.GamePayoutResult(dealer, player));
            }
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

        public List<PlayerStrategy> PlayersAction(DealerStrategy dealer, List<PlayerStrategy> players, Shooter shooter)
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
                    result = PlayerActionMain(dealer, shooter, result, player);
                }
                players.RemoveAt(0);
            }

            return result;
        }

        private List<PlayerStrategy> PlayerActionMain(DealerStrategy dealer, Shooter shooter, List<PlayerStrategy> result, PlayerStrategy player)
        {
            var action = PlayerAction(player, dealer.UpCard, shooter);
            if (action == HandAction.Split)
            {
                List<PlayerStrategy> splitTree = SplitTreeAction(dealer, shooter, player);
                result.AddRange(splitTree.Where(usePlayer => !usePlayer.IsNull));
            }
            else
            {
                result.Add(player);
            }
            return result;
        }

        private List<PlayerStrategy> SplitTreeAction(DealerStrategy dealer, Shooter shooter, PlayerStrategy player)
        {
            var result = CreateSplitTree(player, shooter);
            for (var i = 0; i < result.Count; i++)
            {
                var strategy = result[i];
                if (strategy.IsNull) continue;
                // blankCardがあったら作りなおす
                if (strategy.IsBlank)
                {
                    result[i] = CreatePlayerStrategy(strategy.FirstCard, shooter.Pull(), i + 1);
                    strategy = result[i];
                }
                PlayerActionBySplit(dealer.UpCard, shooter, strategy, i, result);
            }
            return result;
        }

        private List<PlayerStrategy> CreateSplitTree(PlayerStrategy player, Shooter shooter)
        {
            var result = new List<PlayerStrategy>();
            for (var i = 0; i < Rule.MaxSplit; i++)
            {
                result.Add(CreateNullPlayerStrategy(player.FirstCard, i + 1));
            }
            result[0] = CreatePlayerStrategy(player.FirstCard, shooter.Pull(), 1);
            result[1] = CreatePlayerStrategy(player.SecondCard, BlankCard.Build(), 2);
            return result;
        }

        private void PlayerActionBySplit(Card upCard, Shooter shooter, PlayerStrategy player, int treeIndex, List<PlayerStrategy> splitTree)
        {
            var action = PlayerAction(player, upCard, shooter);
            if (splitTree.All(strategy => !strategy.IsNull)) return;
            if (action == HandAction.Split)
            {
                var tempSecondCard = player.SecondCard;
                splitTree[treeIndex] = CreatePlayerStrategy(player.FirstCard, shooter.Pull(), treeIndex);
                player = splitTree[treeIndex];
                // Splitされた次のboxのためにnullStrategyではなくBlankCardを持つデータを作っておく
                int nextIndex = splitTree.FindIndex(split => split.IsNull);
                splitTree[nextIndex] = CreatePlayerStrategy(tempSecondCard, BlankCard.Build(), nextIndex + 1);
                if (splitTree.All(strategy => !strategy.IsNull)) splitTree.ForEach(strategy => strategy.IsMaxSplitTree = true);
                // ここは次のカードではなく今のカードのアクションをしなければいけない
                PlayerActionBySplit(upCard, shooter, player, treeIndex, splitTree);
            }
        }

        public void DealerPreAction(DealerStrategy dealer, List<PlayerStrategy> players)
        {
            var judge = new Judgment(Rule);
            judge.PreActionPlayers(dealer, players);
        }

        // 再帰以外で返るのはHandAction.Stand or HandAction.Surrender or HandAction.Split
        public HandAction PlayerAction(PlayerStrategy player, Card upCard, Shooter shooter)
        {
            HandAction result = player.Action(upCard);
            if (player.CanNotHit() || result == HandAction.Stand) return HandAction.Stand;
            if (IsPlayerActionSurrender(Rule, player, result)) return HandAction.Surrender;
            if (result == HandAction.Split) return HandAction.Split;
            if (IsPlayerActionDoubleDown(player, result))
            {
                player.DoubleDown(shooter.Pull());
                return HandAction.Stand;
            }
            // TODO: ここの自動テストがない
            if(result == HandAction.StandOrDoubleDown) return HandAction.Stand;
            // TODO: Hit SurrenderType == No でもHitOrSurrenderしかないのでここで強制的に引く
            player.Hit(shooter.Pull());
            return PlayerAction(player, upCard, shooter);
        }

        // 先頭がdealerのハンド、残りがプレーヤーのハンドのstrategyリストを返す
        public List<AbstractStrategy> FirstDeal(Shooter shooter, int boxCount)
        {
            var result = new List<AbstractStrategy>();
            var dealHand = FirstDealCardPull(shooter, boxCount);
            var halfCount = dealHand.Count / 2;
            // もしboxCount = 2の場合
            // 0, 1, 2*,3,4,5*
            // もしboxCount = 3の場合
            // 0, 1, 2,3*,4,5,6,7*
            result.Add(CreateDealerStrategy(dealHand[halfCount - 1], dealHand[^1]));
            for (int i = 0; i < boxCount; i++)
            {
                result.Add(CreatePlayerStrategy(dealHand[i], dealHand[i + halfCount]));
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
            return (result == HandAction.HitOrDoubleDown || result == HandAction.DoubleDown || result == HandAction.StandOrDoubleDown) && player.CanDoubleDown();
        }

        // 任意のfactoryに差し替える仕組みをいれることによって各アクションの多様性を担保できる
        protected virtual DealerStrategy CreateDealerStrategy(Card firstCard, Card secondCard) => StrategyFactory.BuildDealer(firstCard, secondCard, Rule);
        // player strategy Create
        protected virtual PlayerStrategy CreatePlayerStrategy(Card firstCard, Card secondCard, int splitCount = 0) => StrategyFactory.BuildBasic(firstCard, secondCard, Rule, splitCount: splitCount);
        private static PlayerStrategy CreateNullPlayerStrategy(Card firstCard, int splitCount) => StrategyFactory.BuildNullPlayer(firstCard, splitCount);
    }
}

