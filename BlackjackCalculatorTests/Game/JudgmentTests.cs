using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackjackCalculator.Factory;
using BlackjackCalculator.Game;
using BlackjackCalculator.Cards;
using BlackjackCalculatorTests.Strategy.Mock;

namespace BlackjackCalculator.Game.Tests
{
    [TestClass()]
    public class JudgmentTests
    {
        [TestMethod()]
        public void PreActionUpCardAceTest()
        {
            var judgement = new Judgment(RuleFactory.BuildBasicRule());
            // BJ vs 19 No
            var dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Ten);
            var player = MockPlayerStrategy.Build(Card.Ten, Card.Nine, GamePreAction.No);
            Assert.AreEqual(GameResult.Lose, judgement.PreActionUpCardAce(dealer, player));

            // 20 vs 19 No
            dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Nine);
            player = MockPlayerStrategy.Build(Card.Ten, Card.Nine, GamePreAction.No);
            Assert.AreEqual(GameResult.No, judgement.PreActionUpCardAce(dealer, player));

            // BJ vs BJ No
            dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Ten);
            player = MockPlayerStrategy.Build(Card.Queen, Card.Ace, GamePreAction.No);
            Assert.AreEqual(GameResult.Push, judgement.PreActionUpCardAce(dealer, player));

            // BJ vs BJ EvenMoney
            dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Ten);
            player = MockPlayerStrategy.Build(Card.Queen, Card.Ace, GamePreAction.EvenMoney);
            Assert.AreEqual(GameResult.Win, judgement.PreActionUpCardAce(dealer, player));

            // BJ vs 20 Insurance
            dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Ten);
            player = MockPlayerStrategy.Build(Card.Queen, Card.Jack, GamePreAction.Insurance);
            Assert.AreEqual(GameResult.Push, judgement.PreActionUpCardAce(dealer, player));

            // BJ vs 20 Surrender
            dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Ten);
            player = MockPlayerStrategy.Build(Card.Ten, Card.King, GamePreAction.Surrender);
            Assert.AreEqual(GameResult.LoseBySurrender, judgement.PreActionUpCardAce(dealer, player));
            // 20 vs 20 Surrender
            dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Nine);
            player = MockPlayerStrategy.Build(Card.Ten, Card.King, GamePreAction.Surrender);
            Assert.AreEqual(GameResult.LoseBySurrender, judgement.PreActionUpCardAce(dealer, player));
        }

        [TestMethod()]
        public void PreActionUpCardNotAceTest()
        {
            var judgement = new Judgment(RuleFactory.BuildBasicRule());
            // BJ vs 19 No
            var dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Ace);
            var player = MockPlayerStrategy.Build(Card.Ten, Card.Nine, GamePreAction.No);
            Assert.AreEqual(GameResult.Lose, judgement.PreActionUpCardNotAce(dealer, player));
            // 20 vs 20 No
            dealer = StrategyFactory.BuildDealer(Card.Queen, Card.Ten);
            player = MockPlayerStrategy.Build(Card.Ten, Card.Nine, GamePreAction.No);
            Assert.AreEqual(GameResult.No, judgement.PreActionUpCardNotAce(dealer, player));
            // 20 vs 21 No
            dealer = StrategyFactory.BuildDealer(Card.Queen, Card.Ten);
            player = MockPlayerStrategy.Build(Card.Ten, Card.Ace, GamePreAction.No);
            Assert.AreEqual(GameResult.No, judgement.PreActionUpCardNotAce(dealer, player));

            // 20 vs 20 surrender
            dealer = StrategyFactory.BuildDealer(Card.Queen, Card.Nine);
            player = MockPlayerStrategy.Build(Card.Ten, Card.Nine, GamePreAction.Surrender);
            Assert.AreEqual(GameResult.LoseBySurrender, judgement.PreActionUpCardNotAce(dealer, player));

        }
    }
}