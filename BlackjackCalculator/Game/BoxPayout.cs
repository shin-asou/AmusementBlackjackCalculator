using BlackjackCalculator.Strategy;

namespace BlackjackCalculator.Game
{
    public class BoxPayout(RuleSet rule, DealerStrategy dealer, PlayerStrategy player, GameResult result, decimal betSize = 600m)
    {
        protected RuleSet Rule { get; } = rule;
        protected DealerStrategy Dealer { get; } = dealer;
        protected PlayerStrategy Player { get; } = player;
        public bool IsSuccessInsurance { get; set; } = false;
        protected decimal BetSize { get; } = betSize;

        public GameResult Result = result;
        public bool IsWin
        {
            get
            {
                return GameResult.Win == Result || GameResult.WinByBlackjack == Result ||
                    GameResult.WinByAce2Six == Result ||
                    GameResult.WinBySixUnder == Result || GameResult.WinBySevenUnder == Result || GameResult.WinByEightUnder == Result ||
                    GameResult.WinByStraight == Result || GameResult.WinByThreeSeven == Result;
            }
        }
        public bool IsDealerBlackjack => Dealer.IsBlackjack;
        public bool IsPlayerBlakjack => Player.IsBlackjack;
        public bool IsPushBlackjack => Dealer.IsBlackjack && Player.IsBlackjack;

        public decimal Payout()
        {
            var payoutTable = Rule.ResultPayout[Result];
            var doubledown = Player.IsDoubleDown ? 2 : 1;
            return ((BetSize * payoutTable.Multiplier) * doubledown) + payoutTable.Bonus;
        }
    }
}
