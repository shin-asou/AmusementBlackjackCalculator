using BlackjackCalculator.Factory;
using BlackjackCalculator.Game;
using BlackjackCalculator.Item;

namespace BlackjackCalculator.Strategy.Tests
{
    [TestClass()]
    public class DealerStrategyTests
    {
        [TestMethod()]
        public void ActionTest()
        {
            // Blackjack
            var dealer = StrategyFactory.BuildDealer(Card.Ten, Card.Ace);
            Assert.AreEqual(HandAction.Stand, dealer.Action());
            // Hard 17
            dealer = StrategyFactory.BuildDealer(Card.Ten, Card.Seven);
            Assert.AreEqual(HandAction.Stand, dealer.Action());
            // 16
            dealer = StrategyFactory.BuildDealer(Card.Nine, Card.Seven);
            Assert.AreEqual(HandAction.Hit, dealer.Action());
            // Soft 17
            dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Six);
            Assert.AreEqual(HandAction.Stand, dealer.Action());
            // Soft 16
            dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Five);
            Assert.AreEqual(HandAction.Hit, dealer.Action());

            dealer = StrategyFactory.BuildDealer(Card.Seven, Card.Four);
            Assert.AreEqual(HandAction.Hit, dealer.Action());
            dealer.Hit(Card.Six);
            Assert.AreEqual(HandAction.Stand, dealer.Action());

            // DealerSoftHand17 Hit
            dealer = StrategyFactory.BuildDealer(Card.Six, Card.Ace, RuleFactory.Build(RuleFactory.BuildBasicPayoutTable(), isStandValueDealerSoftHand: false));
            Assert.AreEqual(HandAction.Hit, dealer.Action());
            dealer = StrategyFactory.BuildDealer(Card.Two, Card.Ace, RuleFactory.Build(RuleFactory.BuildBasicPayoutTable(), isStandValueDealerSoftHand: false));
            dealer.Hit(Card.Four);
            Assert.AreEqual(HandAction.Hit, dealer.Action());
            // DealerSoftHand17 Hit Hardhand
            dealer = StrategyFactory.BuildDealer(Card.Ten, Card.Seven, RuleFactory.Build(RuleFactory.BuildBasicPayoutTable(), isStandValueDealerSoftHand: false));
            Assert.AreEqual(HandAction.Stand, dealer.Action());

        }
        [TestMethod()]
        public void ResultTest()
        {
            // Blackjack
            var dealer = StrategyFactory.BuildDealer(Card.Ten, Card.Ace);
            Assert.AreEqual(HandResult.Blackjack, dealer.Result());
            // value test
            dealer = StrategyFactory.BuildDealer(Card.Ten, Card.Seven);
            Assert.AreEqual(HandResult.Value, dealer.Result());
            // 22
            dealer.Hit(Card.Five);
            Assert.AreEqual(HandResult.Burst, dealer.Result());
            // 21
            dealer = StrategyFactory.BuildDealer(Card.Six, Card.Seven);
            dealer.Hit(Card.Eight);
            Assert.AreEqual(HandResult.Value, dealer.Result());
        }
        [TestMethod()]
        public void UpCardTest()
        {
            // Blackjack
            var dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Ten);
            Assert.AreEqual(Card.Ace, dealer.UpCard);
            dealer = StrategyFactory.BuildDealer(Card.Nine, Card.Ace);
            Assert.AreEqual(Card.Nine, dealer.UpCard);
        }

        // exception
        [TestMethod()]
        public void ActionExceptionTest()
        {
            // Blackjack
            var dealer = StrategyFactory.BuildDealer(Card.Ten, Card.Ace);
            Assert.ThrowsException<NotSupportedException>(() => dealer.Action(Card.Seven));
        }
    }
}