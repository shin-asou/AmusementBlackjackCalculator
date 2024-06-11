using BlackjackCalculator.Factory;
using BlackjackCalculator.Game;
using BlackjackCalculator.Item;
using BlackjackCalculator.Strategy;

namespace BlackjackCalculatorTests.Strategy.Mock
{
    // PlayerStrategyクラスのメソッドをテストするためのモック
    internal class MockPlayerStrategy(Hand hand, RuleSet rule, GamePreAction preAction = GamePreAction.No, HandAction pairAction = HandAction.Split, HandAction notPairAction = HandAction.Stand) : PlayerStrategy(hand, rule)
    {
        private readonly GamePreAction _preAction = preAction;
        private readonly HandAction _pairAction = pairAction;
        private readonly HandAction _notPairAction = notPairAction;

        protected override HandAction ActionByPairHand(Card upCard) => _pairAction;
        protected override HandAction ActionByNotPairHand(Card upCard) => _notPairAction;
        protected override GamePreAction GamePreActionProc(Card upCard) => _preAction;
        public static MockPlayerStrategy Build(Card firstCard, Card secondCard, int splitCount, GamePreAction preAction = GamePreAction.No, HandAction pairAction = HandAction.Split, HandAction notPairAction = HandAction.Stand)
        {
            return new MockPlayerStrategy(HandFactory.Build(firstCard, secondCard, splitCount), RuleFactory.BuildBasicRule(), preAction, pairAction, notPairAction);
        }
        public static MockPlayerStrategy Build(Card firstCard, Card secondCard, GamePreAction preAction = GamePreAction.No, HandAction pairAction = HandAction.Split, HandAction notPairAction = HandAction.Stand)
        {
            return Build(firstCard, secondCard, 0, preAction, pairAction, notPairAction);
        }
    }
}
