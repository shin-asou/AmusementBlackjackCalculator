using BlackjackCalculator.Game;
using BlackjackCalculator.Item;

namespace BlackjackCalculator.Strategy
{
    public class NullPlayerStrategy(Hand hand, RuleSet rule) : PlayerStrategy(hand, rule)
    {
        public override bool IsNull => true;
        protected override HandAction ActionByPairHand(Card upCard) => HandAction.Stand;
        protected override HandAction ActionByNotPairHand(Card upCard) => HandAction.Stand;
    }
}
