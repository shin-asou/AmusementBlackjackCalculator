﻿using BlackjackCalculator.Game;
using System.Collections.Frozen;

namespace BlackjackCalculator.Factory
{
    public static class RuleFactory
    {
        // 葛西H3ALルール
        // https://x.com/AmusementH3al
        public static RuleSet BuildH3ALRule()
        {
            return new RuleSet(
                isStandValueDealerSoftHand: true,
                canDoubledownAfterSplit: true,
                maxSplit: 4,
                canResplitAces: false,
                canHitSplitAces: false,
                isMadeBySplitBlackjackNatural: true,
                doubledown: DoubledownType.AnyTwoCard,
                surrenderType: SurrenderType.Late,
                payoutTable: BuildAllValidHandPayoutTable(),
                deckCount: 6,
                endDeckCount: 1);
        }
        public static RuleSet BuildBasicRule()
        {
            return new RuleSet(
                isStandValueDealerSoftHand: true,
                canDoubledownAfterSplit: true,
                maxSplit: 4,
                canResplitAces: false,
                canHitSplitAces: false,
                isMadeBySplitBlackjackNatural: false,
                doubledown: DoubledownType.AnyTwoCard,
                surrenderType: SurrenderType.Late,
                payoutTable: BuildBasicPayoutTable(),
                deckCount: 6,
                endDeckCount: 1);
        }
        public static FrozenDictionary<GameResult, ResultPayout> BuildBasicPayoutTable()
        {
            return new Dictionary<GameResult, ResultPayout>()
            {
                { GameResult.WinByBlackjack, new ResultPayout(true, 1.5m, 0 )},
                { GameResult.Win, new ResultPayout(true, 1, 0 )},
                { GameResult.Lose, new ResultPayout(true, -1, 0 )},
                { GameResult.LoseBySurrender, new ResultPayout(true, -0.5m, 0 )},
                { GameResult.Push, new ResultPayout(true, 0, 0 )},
                { GameResult.WinBySixUnder, new ResultPayout(false, 2, 0 )},
                { GameResult.WinBySevenUnder, new ResultPayout(false, 4, 0 )},
                { GameResult.WinByEightUnder, new ResultPayout(false, 6, 0 )},
                { GameResult.WinByStraight, new ResultPayout(false, 3, 0 )},
                { GameResult.WinByThreeSeven, new ResultPayout(false, 3, 1000 )},
                { GameResult.WinByAce2Six, new ResultPayout(false, 20, 5000 )},
            }.ToFrozenDictionary();
        }
        public static FrozenDictionary<GameResult, ResultPayout> BuildAllValidHandPayoutTable()
        {
            return new Dictionary<GameResult, ResultPayout>()
            {
                { GameResult.WinByBlackjack, new ResultPayout(true, 1.5m, 0 )},
                { GameResult.Win, new ResultPayout(true, 1, 0 )},
                { GameResult.Lose, new ResultPayout(true, -1, 0 )},
                { GameResult.LoseBySurrender, new ResultPayout(true, -0.5m, 0 )},
                { GameResult.Push, new ResultPayout(true, 0, 0 )},
                { GameResult.WinBySixUnder, new ResultPayout(true, 2, 0 )},
                { GameResult.WinBySevenUnder, new ResultPayout(true, 4, 0 )},
                { GameResult.WinByEightUnder, new ResultPayout(true, 6, 0 )},
                { GameResult.WinByStraight, new ResultPayout(true, 3, 0 )},
                { GameResult.WinByThreeSeven, new ResultPayout(true, 3, 1000 )},
                { GameResult.WinByAce2Six, new ResultPayout(true, 20, 5000 )},
            }.ToFrozenDictionary();
        }
        public static RuleSet Build(
                FrozenDictionary<GameResult, ResultPayout> payoutTable,
                bool isStandValueDealerSoftHand = true,
                bool canDoubledownAfterSplit = true,
                int maxSplit = 4,
                bool canResplitAces = false,
                bool canHitSplitAces = false,
                bool isMadeBySplitBlackjackPayoutAddon = false,
                DoubledownType doubledown = DoubledownType.AnyTwoCard,
                SurrenderType surrenderType = SurrenderType.Late,
                int deckCount = 6,
                int endDeckCoun = 1)
        {
            return new RuleSet(
                isStandValueDealerSoftHand: isStandValueDealerSoftHand,
                canDoubledownAfterSplit: canDoubledownAfterSplit,
                maxSplit: maxSplit,
                canResplitAces: canResplitAces,
                canHitSplitAces: canHitSplitAces,
                isMadeBySplitBlackjackNatural: isMadeBySplitBlackjackPayoutAddon,
                doubledown: doubledown,
                surrenderType: surrenderType,
                payoutTable: payoutTable,
                deckCount: deckCount,
                endDeckCount: endDeckCoun);
        }
    }
}
