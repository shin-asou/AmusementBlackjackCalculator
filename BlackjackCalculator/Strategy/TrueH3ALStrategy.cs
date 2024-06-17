using BlackjackCalculator.Game;
using BlackjackCalculator.Item;

namespace BlackjackCalculator.Strategy
{
    // 葛西H3ALのBlackjackを攻略するための真のベーシックストラテジー
    // https://x.com/AmusementH3al
    public class TrueH3ALStrategy(Hand hand, RuleSet rule) : BasicStrategy(hand, rule)
    {
        protected override HandAction ActionByNotPairHand(Card upCard)
        {
            // 6Underリーチでディーラー10 or Aの場合引いてみる
            if((upCard.IsTen || upCard.IsAce) && (Hand.Count == 5 && Hand.Value() == 17)) return HandAction.Hit;
            // Ace to Sixは20倍で必ずヒットが肯定されるので引く
            if (Hand.IsReachAce2Six) return HandAction.Hit;
            // 7/13で飛ばずかつ配当が2x => 4xなので期待値プラスなので引く
            if (Hand.Count == 6 && Hand.MinValue() <= 14) return HandAction.Hit;
            // xUnderにリーチがかかり次が飛ばない場合必ず引く
            if (Hand.Count >= 5 && Hand.MinValue() <= 11) return HandAction.Hit;
            return base.ActionByNotPairHand(upCard);
        }
    }
}
