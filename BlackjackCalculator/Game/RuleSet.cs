﻿using System.Collections.Frozen;

namespace BlackjackCalculator.Game
{
    // 標準的なBlackjackのルールを定義する
    // odds calculatorの内容をベースにアミューズ系を追加
    // 参考:https://wizardofodds.com/games/blackjack/calculator/
    public class RuleSet(bool isStandDealerSandSoftHand, bool canDoubledownAfterSplit, int maxSplit, bool canResplitAces, bool canHitSplitAces, DoubledownType doubledown, SurrenderType surrenderType, FrozenDictionary<GameResult, ResultPayout> payoutTable, int deckCount, int endDeckCount)
    {
        public bool IsStandDealerSandSoftHand { get; } = isStandDealerSandSoftHand;
        public bool CanDoubledownAfterSplit { get; } = canDoubledownAfterSplit;
        public int MaxSplit { get; } = maxSplit;
        public bool CanResplitAces { get; } = canResplitAces;
        public bool CanHitSplitAces { get; } = canHitSplitAces;
        public DoubledownType Doubledown { get; } = doubledown;
        public SurrenderType Surrender { get; } = surrenderType;
        public FrozenDictionary<GameResult, ResultPayout> ResultPayout { get; } = payoutTable;
        public int DeckCount { get; } = deckCount;
        public int EndDeckCount { get; } = endDeckCount;
    }
    public readonly record struct ResultPayout(bool Valid, decimal Multiplier, decimal Bonus)
    {
        // payoutMultiplierは元手もありの入力とする　(等倍配当を2.0とみなす)
    }
}
