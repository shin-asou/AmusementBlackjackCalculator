using BlackjackCalculator.Cards;
using BlackjackCalculatorTests.Strategy.Mock;

namespace BlackjackCalculator.Strategy.Tests
{
    [TestClass()]
    public class PlayerStrategyTests
    {
        [TestMethod()]
        public void ActionExceptionTest()
        {
            var player = MockPlayerStrategy.Build(Card.Ten, Card.Ace);
            Assert.ThrowsException<NotSupportedException>(() => player.Action());
        }
    }
}