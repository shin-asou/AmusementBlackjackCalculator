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
        public override HandAction Action(Card upCard)
        {
            if (IsAcesSplit1CardOnly()) return HandAction.Stand;
            if (Hand.IsPair) return PairHandFlow(upCard);
            return ActionByNotPairHand(upCard);
        }
        private HandAction PairHandFlow(Card upCard)
        {
            var result = ActionByPairHand(upCard);
            if (result == HandAction.Split)
            {
                if (CanSplitHand()) return result;
                // splitできない場合はペアでないハンドと同じ扱いをすることとする
                return ActionByNotPairHand(upCard);
            }
            return result;
        }
        protected abstract HandAction ActionByPairHand(Card upCard);
        protected abstract HandAction ActionByNotPairHand(Card upCard);
        public override HandResult Result()
        {
            if (Hand.IsBlackjack) return HandResult.Blackjack;
            return (Hand.IsBurst) ? HandResult.Burst : HandResult.Value;
        }
        protected bool IsAcesSplit1CardOnly() => Hand.IsMadeByAcesSplit && !Rule.CanHitSplitAces;
        protected bool CanSplitHand()
        {
            if (Hand.IsMadeByAcesSplit && !Rule.CanResplitAces) return false;
            return Hand.SplitCount < Rule.MaxSplit;
        }
        public GamePreAction PreAction(Card upCard)
        {
            PreActionResult = GamePreActionProc(upCard);
            return PreActionResult;
        }
        protected virtual GamePreAction GamePreActionProc(Card upCard) => GamePreAction.No;
    }
}