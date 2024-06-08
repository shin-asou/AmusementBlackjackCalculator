using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;

namespace BlackjackCalculator.Strategy
{
    public abstract class PlayerStrategy(Hand hand, RuleSet rule) : AbstractStrategy(hand, rule)
    {
        public Card FirstCard => Hand.FirstCard;
        public Card SecondCard => Hand.SecondCard;
        public GamePreAction PreActionResult { get; private set; } = GamePreAction.No;
        public bool IsEvenMoney => PreActionResult == GamePreAction.EvenMoney;
        public bool IsInsurance => PreActionResult == GamePreAction.Insurance;
        public bool IsPreActionSurrender => PreActionResult == GamePreAction.Surrender;
        // Player は必ずupCardを知っているためupCardを取らないメソッドをサポートしない
        public override HandAction Action() => throw new NotSupportedException();
        public override HandResult Result()
        {
            if (Hand.IsBlackjack) return HandResult.Blackjack;
            return (Hand.IsBurst) ? HandResult.Burst : HandResult.Value;
        }

        public GamePreAction PreAction(Card upCard)
        {
            PreActionResult = GamePreActionProc();
            return PreActionResult;
        }
        protected virtual GamePreAction GamePreActionProc() => GamePreAction.No;
    }
}