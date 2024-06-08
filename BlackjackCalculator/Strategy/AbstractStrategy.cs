using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;

namespace BlackjackCalculator.Strategy
{
    public abstract class AbstractStrategy(Hand hand, RuleSet rule)
    {
        protected RuleSet Rule { get; } = rule;
        protected Hand Hand { get; } = hand;
        public bool IsBlackjack => Hand.IsBlackjack;
        public bool IsNoBlackjack => !Hand.IsBlackjack;
        public abstract HandAction Action();
        public abstract HandAction Action(Card upCard);
        public abstract HandResult Result();
        public void Hit(Card card) { Hand.Hit(card); }
    }
}
