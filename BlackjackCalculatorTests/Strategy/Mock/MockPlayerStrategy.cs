using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;
using BlackjackCalculator.Strategy;

namespace BlackjackCalculatorTests.Strategy.Mock
{
    // PlayerStrategyクラスのメソッドをテストするためのモック
    internal class MockPlayerStrategy(Hand hand, RuleSet rule) : PlayerStrategy(hand, rule)
    {
        public override HandAction Action(Card upCard) { return HandAction.Stand; }
        public override HandResult Result() { return HandResult.Value; }
    }
}
