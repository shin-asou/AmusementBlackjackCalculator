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

    }
}

