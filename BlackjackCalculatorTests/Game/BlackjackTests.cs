using BlackjackCalculator.Factory;
using BlackjackCalculator.Strategy;

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
            Assert.AreEqual((boxCount + 1) * 2, cards.Count);
        }

        [TestMethod()]
        public void FirstDealTest()
        {
            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildShooter(rule.DeckCount, rule.EndDeckCount);
            var boxCount = 4;
            var strategies = Blackjack.FirstDeal(shooter, boxCount);
            Assert.AreEqual((boxCount + 1), strategies.Count);
            // 先頭はDealerStrategyで残りはPlayerStrategyである(もしくはその継承クラス)
            for (int i = 0; i < strategies.Count; i++)
            {
                var strategy = strategies[i];
                Assert.IsTrue((i == 0) ? strategy is DealerStrategy : strategy is PlayerStrategy);
            }
        }
    }
}