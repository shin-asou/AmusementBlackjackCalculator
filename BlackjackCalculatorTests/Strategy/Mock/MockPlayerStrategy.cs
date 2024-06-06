using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;
using BlackjackCalculator.Strategy;

namespace BlackjackCalculatorTests.Strategy.Mock
{
    // PlayerStrategyクラスのメソッドをテストするためのモック
    internal class MockPlayerStrategy(Hand hand) : PlayerStrategy(hand)
    {
        public override HandAction Action(Card upCard) { return HandAction.Stand; }
        public override HandResult Result() { return HandResult.Value; }
    }
}
