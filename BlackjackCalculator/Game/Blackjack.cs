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
        ThreeOfaKind,
        Ace2Six
    }
    public enum GameResult
    {
        WinByBlackjack,
        Win,
        Lose,
        Push,
        WinBySixUnder,
        WinByStraight,
        WinByThreeSeven,
        WinByAce2Six
    }

    public class Blackjack
    {

    }
}

