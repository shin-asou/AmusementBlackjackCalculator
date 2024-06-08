using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;

namespace BlackjackCalculator.Strategy
{
    public partial class BasicStrategy(Hand hand, RuleSet rule) : PlayerStrategy(hand, rule)
    {
        public override HandAction Action(Card upCard)
        {
            if (Hand.IsPair)
            {
                var result = ReferencePairHandStrategy(upCard);
                if (result == HandAction.Split)
                {
                    // TODO: splitの場合はルール的にスプリットできるかをチェックする必要がある 
                    // AAのSplitで1CardOnlyの場合やSplit回数限度が決まっている場合
                    // 上記の懸念は置いといてテストを先に書くのが良い可能性
                    throw new NotImplementedException();
                }
                return result;
            }
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
