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
            result.Add(StrategyFactory.BuildDealer(dealHand[halfCount - 1], dealHand[^1]));
            for (int i = 0; i < boxCount; i++)
            {
                // TODO: ここのストラテジーを任意のPlayerStrategyに切り替えられる仕組みが必要
                result.Add(StrategyFactory.BuildBasic(dealHand[i], dealHand[i + halfCount]));
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
    }
}

