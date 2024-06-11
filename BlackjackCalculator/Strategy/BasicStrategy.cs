using BlackjackCalculator.Game;
using BlackjackCalculator.Item;

namespace BlackjackCalculator.Strategy
{
    public partial class BasicStrategy(Hand hand, RuleSet rule) : PlayerStrategy(hand, rule)
    {
        protected override HandAction ActionByPairHand(Card upCard)
        {
            return ReferencePairHandStrategy(upCard);
        }
        protected override HandAction ActionByNotPairHand(Card upCard)
        {
            return Hand.IsSoft ? ReferenceSoftHandStrategy(upCard) : ReferenceHardHandStrategy(upCard);
        }

        // ストラテジー表参照メソッド　２次元連想配列を作りやすいようにKey Valueを作ってしまったので利用するときのミスを避けるためにラップする
        private HandAction ReferenceHardHandStrategy(Card upCard) => hardHandStrategy[upCard.Value][ClampHardHandValue(Hand.Value())];
        private HandAction ReferenceSoftHandStrategy(Card upCard) => softHandStrategy[upCard.Value][ClampSoftHandValue(Hand.SoftHandPairValue())];
        // ペアハンドの場合はどちらも同じカードのはずなので適当にupCardの値を取るとした
        // このメソッドもペアハンド以外で呼び出せないメソッドになってしまった
        private HandAction ReferencePairHandStrategy(Card upCard) => pairHandStrategy[Hand.UpCard.Value][upCard.Value];
    }
}
