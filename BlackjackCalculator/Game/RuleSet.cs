using System.Collections.Frozen;

namespace BlackjackCalculator.Game
{
    // 標準的なBlackjackのルールを定義する
    public class RuleSet(SurrenderType surrenderType, FrozenDictionary<GameResult, ResultPayout> payoutTable, int deckCount, int endDeckCount)
    {
        public SurrenderType Surrender { get; } = surrenderType;
        public FrozenDictionary<GameResult, ResultPayout> ResultPayout { get; } = payoutTable;
        public int DeckCount { get; } = deckCount;
        public int EndDeckCount { get; } = endDeckCount;
    }
}
