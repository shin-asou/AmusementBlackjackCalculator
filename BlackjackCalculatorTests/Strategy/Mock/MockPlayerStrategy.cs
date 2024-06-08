using BlackjackCalculator.Cards;
using BlackjackCalculator.Factory;
using BlackjackCalculator.Game;
using BlackjackCalculator.Strategy;

namespace BlackjackCalculatorTests.Strategy.Mock
{
    // PlayerStrategyクラスのメソッドをテストするためのモック
    internal class MockPlayerStrategy(Hand hand, RuleSet rule, GamePreAction preAction = GamePreAction.No) : PlayerStrategy(hand, rule)
    {
        private readonly GamePreAction _preAction = preAction;
        public override HandAction Action(Card upCard) { return HandAction.Stand; }
        public override HandResult Result() { return HandResult.Value; }
        protected override GamePreAction GamePreActionProc() => _preAction;
        public static MockPlayerStrategy Build(Card firstCard, Card secondCard, GamePreAction preAction = GamePreAction.No)
        {
            return new MockPlayerStrategy(HandFactory.Build(firstCard, secondCard), RuleFactory.BuildBasicRule(), preAction);
        }
    }
}
