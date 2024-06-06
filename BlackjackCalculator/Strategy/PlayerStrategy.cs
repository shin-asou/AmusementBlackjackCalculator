using BlackjackCalculator.Game;

namespace BlackjackCalculator.Strategy
{
    public abstract class PlayerStrategy(Hand hand) : AbstractStrategy(hand)
    {
        // Player は必ずupCardを知っているためupCardを取らないメソッドをサポートしない
        public override HandAction Action() => throw new NotSupportedException();
        public override HandResult Result()
        {
            if (Hand.IsBlackjack) return HandResult.Blackjack;
            return (Hand.IsBurst) ? HandResult.Burst : HandResult.Value;
        }
    }
}
