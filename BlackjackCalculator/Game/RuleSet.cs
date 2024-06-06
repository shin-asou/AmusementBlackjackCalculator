using System.Collections.Frozen;

namespace BlackjackCalculator.Game
{
    // 標準的なBlackjackのルールを定義する
    public class RuleSet(SurrenderType surrenderType, FrozenDictionary<GameResult, ResultPayout> payoutTable)
    {
        public SurrenderType Surrender { get; } = surrenderType;
        public FrozenDictionary<GameResult, ResultPayout> ResultPayout { get; } = payoutTable;
    }
}
