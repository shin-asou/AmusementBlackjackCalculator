using BlackjackCalculator.Game;
using BlackjackCalculator.Item;

namespace BlackjackCalculator.Strategy
{
    public abstract class PlayerStrategy(Hand hand, RuleSet rule) : AbstractStrategy(hand, rule)
    {
        public virtual bool IsNull => false;
        public Card FirstCard => Hand.FirstCard;
        public Card SecondCard => Hand.SecondCard;
        public GamePreAction PreActionResult { get; private set; } = GamePreAction.No;
        // これだけは嫌だが
        public bool IsMaxSplitTree { get; set; }
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
        // ここではルール上採用されているハンドかは無視して設定する
        public override HandResult Result()
        {
            // Ace to Six が先でないと6underと判定されるので注意
            if (Hand.IsAce2Six) return HandResult.Ace2Six;
            if (Hand.IsBlackjack) return HandResult.Blackjack;
            if (Hand.IsStraight) return HandResult.Straight;
            if (Hand.IsThreeSeven) return HandResult.ThreeSeven;
            if (Hand.IsSixUnder) return HandResult.SixUnder;
            if (Hand.IsSevenUnder) return HandResult.SevenUnder;
            if (Hand.IsEightUnder) return HandResult.EightUnder;
            return (Hand.IsBurst) ? HandResult.Burst : HandResult.Value;
        }
        public bool IsSpecialHand => Hand.IsSpecial;

        public bool CanNotHit()
        {
            return (Hand.IsBurst || Hand.IsDoubleDown || IsAcesSplit1CardOnly() || Hand.IsBlackjack);
        }
        public bool CanDoubleDown()
        {
            return !(
                (CanNotHit() || !Hand.FirstDeal) ||
                (!Rule.CanDoubledownAfterSplit && Hand.IsMadeBySplit) ||
                (Rule.Doubledown == DoubledownType.Ten2Eleven && (Math.Clamp(Hand.Value(), 10, 11) != Hand.Value())) ||
                (Rule.Doubledown == DoubledownType.Nine2Eleven && (Math.Clamp(Hand.Value(), 9, 11) != Hand.Value()))
            );
        }
        protected bool IsAcesSplit1CardOnly() => Hand.IsMadeByAcesSplit && !Rule.CanHitSplitAces;
        protected bool CanSplitHand()
        {
            if (Hand.IsMadeByAcesSplit && !Rule.CanResplitAces) return false;
            if (IsMaxSplitTree) return false;
            return Hand.SplitCount < Rule.MaxSplit;
        }
        public bool CanSurrender => Hand.FirstDeal;

        public bool IsEndByPreAction()
        {
            return PreActionResult == GamePreAction.EvenMoney ||
                PreActionResult == GamePreAction.Surrender;
        }

        public GamePreAction PreAction(Card upCard)
        {
            PreActionResult = GamePreActionProc(upCard);
            return PreActionResult;
        }
        protected virtual GamePreAction GamePreActionProc(Card upCard) => GamePreAction.No;

        public void DoubleDown(Card card) => Hand.DoubleDown(card);
    }
}