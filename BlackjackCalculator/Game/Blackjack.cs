using BlackjackCalculator.Cards;
using BlackjackCalculator.Factory;
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
        // ---- プレイヤーごとのアクション
        // ---- プレイヤースプリット処理
        // ---- 結果に応じて配当
        public static void GameSequence(RuleSet rule, DealerStrategy dealer, List<PlayerStrategy> players, Shooter shooter)
        {
            // DealerBlackjack + Early Surrender Check
            DealerPreAction(rule, dealer, players);
            PlayerAction(dealer, players, shooter);
        }

        private static void PlayerAction(DealerStrategy dealer, List<PlayerStrategy> players, Shooter shooter)
        {
            while (players.Count > 0)
            {
                var player = players.First();
                HandAction action = PlayerAction(player, dealer.UpCard, shooter);
                if (action == HandAction.Split)
                {
                    players.Insert(1, CreatePlayerStrategy(player.FirstCard, shooter.Pull()));
                    // ここはまだ間違っていて1つ目のスプリットハンドを引ききってからpullしないといけない
                    players.Insert(2, CreatePlayerStrategy(player.SecondCard, shooter.Pull()));
                }
                players.RemoveAt(0);
            }
        }

        public static void DealerPreAction(RuleSet rule, DealerStrategy dealer, List<PlayerStrategy> players)
        {
            var judge = new Judgment(rule);
            judge.PreActionPlayers(dealer, players);
        }

        // 再帰以外で返るのはHandAction.Stand or HandAction.Surrender or HandAction.Split
        public static HandAction PlayerAction(PlayerStrategy player, Card upCard, Shooter shooter)
        {
            HandAction result = player.Action(upCard);
            if (result == HandAction.Hit || result == HandAction.HitOrDoubleDown || result == HandAction.DoubleDown)
            {
                player.Hit(shooter.Pull());
                if (HandResult.Burst == player.Result()) return HandAction.Stand;
                return PlayerAction(player, upCard, shooter);
            }
            return result;
        }

        // 先頭がdealerのハンド、残りがプレーヤーのハンドのstrategyリストを返す
        public static List<AbstractStrategy> FirstDeal(Shooter shooter, int boxCount)
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

        // 任意のfactoryに差し替える仕組みをいれることによって各アクションの多様性を担保できる
        protected static DealerStrategy CreateDealerStrategy(Card firstCard, Card secondCard) => StrategyFactory.BuildDealer(firstCard, secondCard);
        // player strategy Create
        protected static PlayerStrategy CreatePlayerStrategy(Card firstCard, Card secondCard) => StrategyFactory.BuildBasic(firstCard, secondCard);

        public static BlackjackRecord CreateBlackjackRecord(int boxCount)
        {
            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildShooter(rule.DeckCount, rule.EndDeckCount);
            var boxes = FirstDeal(shooter, boxCount);
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

