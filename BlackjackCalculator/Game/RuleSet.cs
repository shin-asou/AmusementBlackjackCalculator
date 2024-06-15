using System.Collections.Frozen;

namespace BlackjackCalculator.Game
{
    // 標準的なBlackjackのルールを定義する
    // odds calculatorの内容をベースにアミューズ系を追加
    // 参考:https://wizardofodds.com/games/blackjack/calculator/
    public class RuleSet(bool isStandValueDealerSoftHand, bool canDoubledownAfterSplit, int maxSplit, bool canResplitAces, bool canHitSplitAces, bool isMadeBySplitBlackjackPayoutAddon, DoubledownType doubledown, SurrenderType surrenderType, FrozenDictionary<GameResult, ResultPayout> payoutTable, int deckCount, int endDeckCount)
    {
        public bool IsStandValueDealerSoftHand { get; } = isStandValueDealerSoftHand;
        public bool CanDoubledownAfterSplit { get; } = canDoubledownAfterSplit;
        public int MaxSplit { get; } = maxSplit;
        public bool CanResplitAces { get; } = canResplitAces;
        public bool CanHitSplitAces { get; } = canHitSplitAces;
        public bool IsMadeBySplitBlackjackPayoutAddon { get; } = isMadeBySplitBlackjackPayoutAddon;
        public DoubledownType Doubledown { get; } = doubledown;
        public SurrenderType Surrender { get; } = surrenderType;
        public FrozenDictionary<GameResult, ResultPayout> ResultPayout { get; } = payoutTable;
        public int DeckCount { get; } = deckCount;
        public int EndDeckCount { get; } = endDeckCount;
    }
    public readonly record struct ResultPayout(bool Valid, decimal Multiplier, decimal Bonus)
    {
        // payoutMultiplierは元手なしとする (負けの時を-1として計算)
    }
}
