using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;

namespace BlackjackCalculator.Strategy
{
    public abstract class AbstractStrategy(Hand hand)
    {
        protected Hand Hand { get; } = hand;
        public abstract HandAction Action();
        public abstract HandAction Action(Card upCard);
        public abstract HandResult Result();
        public void Hit(Card card) { Hand.Hit(card); }
    }
}
