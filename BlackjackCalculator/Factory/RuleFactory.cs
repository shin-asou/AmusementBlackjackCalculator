using BlackjackCalculator.Game;
using System.Collections.Frozen;

namespace BlackjackCalculator.Factory
{
    public static class RuleFactory
    {
        public static RuleSet BuildBasicRule()
        {
            var payoutTable = new Dictionary<GameResult, ResultPayout>()
            {
                { GameResult.WinByBlackjack, new ResultPayout(true, 2.5m, 0 )},
                { GameResult.Win, new ResultPayout(true, 2, 0 )},
                { GameResult.Lose, new ResultPayout(true, 0, 0 )},
                { GameResult.LoseBySurrender, new ResultPayout(true, 0.5m, 0 )},
                { GameResult.Push, new ResultPayout(true, 1, 0 )},
                { GameResult.WinBySixUnder, new ResultPayout(false, 3, 0 )},
                { GameResult.WinByStraight, new ResultPayout(false, 4, 0 )},
                { GameResult.WinByThreeSeven, new ResultPayout(false, 4, 1000 )},
                { GameResult.WinByAce2Six, new ResultPayout(false, 4, 5000 )},
            }.ToFrozenDictionary();

            return new RuleSet(SurrenderType.Late, payoutTable, 6, 1);
        }
    }
}
