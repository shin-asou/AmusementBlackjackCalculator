﻿using BlackjackCalculator.Cards;
using BlackjackCalculator.Factory;
using BlackjackCalculatorTests.Strategy.Mock;

namespace BlackjackCalculator.Strategy.Tests
{
    [TestClass()]
    public class PlayerStrategyTests
    {
        [TestMethod()]
        public void ActionExceptionTest()
        {
            var player = new MockPlayerStrategy(HandFactory.Build(Card.Ten, Card.Ace), RuleFactory.BuildBasicRule());
            Assert.ThrowsException<NotSupportedException>(() => player.Action());
        }
    }
}