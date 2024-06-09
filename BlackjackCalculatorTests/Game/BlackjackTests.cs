using BlackjackCalculator.Cards;
using BlackjackCalculator.Factory;
using BlackjackCalculator.Strategy;
using BlackjackCalculatorTests.Strategy.Mock;

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
            var strategies = Blackjack.FirstDeal(rule, shooter, boxCount);
            Assert.AreEqual((boxCount + 1), strategies.Count);
            // 先頭はDealerStrategyで残りはPlayerStrategyである(もしくはその継承クラス)
            for (int i = 0; i < strategies.Count; i++)
            {
                var strategy = strategies[i];
                Assert.IsTrue((i == 0) ? strategy is DealerStrategy : strategy is PlayerStrategy);
            }
        }

        [TestMethod()]
        public void PlayerActionTest()
        {
            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildShooter(rule.DeckCount, rule.EndDeckCount);
            var player = MockPlayerStrategy.Build(Card.Ten, Card.Queen);
            Assert.AreEqual(HandAction.Stand, Blackjack.PlayerAction(rule, player, Card.Ten, shooter));
            // BJ => stand
            player = MockPlayerStrategy.Build(Card.Ten, Card.Ace, notPairAction: HandAction.Hit);
            Assert.AreEqual(HandAction.Stand, Blackjack.PlayerAction(rule, player, Card.Ten, shooter));

            player = MockPlayerStrategy.Build(Card.Nine, Card.Five, notPairAction: HandAction.Hit);
            player.Hit(Card.Queen);
            Assert.AreEqual(HandAction.Stand, Blackjack.PlayerAction(rule, player, Card.Ten, shooter));
        }
    }
}