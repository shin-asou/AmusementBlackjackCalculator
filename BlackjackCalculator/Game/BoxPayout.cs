using BlackjackCalculator.Strategy;

namespace BlackjackCalculator.Game
{
    public class BoxPayout(RuleSet rule, DealerStrategy dealer, PlayerStrategy player, GameResult gameResult)
    {
        protected RuleSet Rule { get; } = rule;
        protected DealerStrategy Dealer { get; } = dealer;
        protected PlayerStrategy Player { get; } = player;
        public bool IsSuccessInsurance { get; set; } = false;

        public GameResult GameResult = gameResult;
    }
}
