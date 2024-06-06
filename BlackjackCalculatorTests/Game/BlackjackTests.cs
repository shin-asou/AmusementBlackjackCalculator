using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackjackCalculator.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackjackCalculator.Factory;
using System.Data;

namespace BlackjackCalculator.Game.Tests
{
    [TestClass()]
    public class BlackjackTests
    {
        [TestMethod()]
        public void FirstDealCardPullTest()
        {
            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildShooter(rule.DeckCount, rule.EndDeckCount);
            var boxCount = 4;
            var cards = Blackjack.FirstDealCardPull(shooter, boxCount);
            Assert.AreEqual((boxCount+1) * 2, cards.Count);
        }
    }
}