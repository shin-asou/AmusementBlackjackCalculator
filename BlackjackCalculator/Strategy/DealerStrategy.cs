using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;

namespace BlackjackCalculator.Strategy
{
    // Dealerのアクションクラス
    // ディーラーは17以上になるまで必ずHitし17以上の場合は必ずStand
    // softhand17(A+6)についてはHitしないものとする
    public class DealerStrategy(Hand hand, RuleSet rule) : AbstractStrategy(hand, rule)
    {
        public Card UpCard => Hand.UpCard;
        public bool IsUpCardAce => Hand.IsUpCardAce;
        public bool IsNotUpCardAce => !IsUpCardAce;

        public override HandAction Action()
        {
            return Hand.IsDealerStand ? HandAction.Stand : HandAction.Hit;
        }
        // DealerはupCardを参照しないためupCardを必要とするメソッドをサポートしない
        public override HandAction Action(Card upCard) => throw new NotSupportedException();
        public override HandResult Result()
        {
            if (Hand.IsBlackjack) return HandResult.Blackjack;
            return (Hand.IsBurst) ? HandResult.Burst : HandResult.Value;
        }
    }
}
