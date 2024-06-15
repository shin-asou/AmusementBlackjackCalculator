using BlackjackCalculator.Factory;
using BlackjackCalculator.Item;
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

        [TestMethod()]
        public void GamePayoutResultTest()
        {
            var judgement = new Judgment(RuleFactory.BuildBasicRule());
            var dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Ace);
            var player = MockPlayerStrategy.Build(Card.Jack, Card.Ace);

            // BJ vs BJ no evenmoney
            player.PreAction(dealer.UpCard);
            var result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Push, result.GameResult);
            // BJ vs BJ evenmoney
            player = MockPlayerStrategy.Build(Card.Jack, Card.Ace, preAction: GamePreAction.EvenMoney);
            player.PreAction(dealer.UpCard);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Win, result.GameResult);
            // BJ vs EarlySurrender
            player = MockPlayerStrategy.Build(Card.Jack, Card.Six, preAction: GamePreAction.Surrender);
            player.PreAction(dealer.UpCard);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.LoseBySurrender, result.GameResult);
            // BJ vs Insurance
            player = MockPlayerStrategy.Build(Card.Jack, Card.Six, preAction: GamePreAction.Insurance);
            player.PreAction(dealer.UpCard);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Push, result.GameResult);
            Assert.IsTrue(result.IsSuccessInsurance);
            // BJ vs No
            player = MockPlayerStrategy.Build(Card.Jack, Card.Eight);
            player.PreAction(dealer.UpCard);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Lose, result.GameResult);

            // dealer no blackjack

            // 20 vs Blackjack
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.King);
            player = MockPlayerStrategy.Build(Card.Ace, Card.Ten);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.WinByBlackjack, result.GameResult);
            // 20 vs 678 to 21 no valid rule
            player = MockPlayerStrategy.Build(Card.Six, Card.Seven);
            player.Hit(Card.Eight);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Win, result.GameResult);
            // 21vs
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Six);
            dealer.Hit(Card.Five);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Push, result.GameResult);
            // 20 vs 777 to 21 no valid rule
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.King);
            player = MockPlayerStrategy.Build(Card.Seven, Card.Seven);
            player.Hit(Card.Seven);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Win, result.GameResult);
            // 21 vs
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Six);
            dealer.Hit(Card.Five);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Push, result.GameResult);
            // 20 vs Ace to Six no valid rule
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.King);
            player = MockPlayerStrategy.Build(Card.Ace, Card.Two);
            player.Hit(Card.Three);
            player.Hit(Card.Four);
            player.Hit(Card.Five);
            player.Hit(Card.Six);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Win, result.GameResult);
            // vs 21
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Six);
            dealer.Hit(Card.Five);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Push, result.GameResult);

            // 20 vs 14 6under no valid rule
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.King);
            player = MockPlayerStrategy.Build(Card.Ace, Card.Two);
            player.Hit(Card.Two);
            player.Hit(Card.Two);
            player.Hit(Card.Three);
            player.Hit(Card.Four);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Lose, result.GameResult);
            // 20 vs 17 7under no valid rule
            player.Hit(Card.Three);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Lose, result.GameResult);
            // 20 vs 17 8under no valid rule
            player.Hit(Card.Three);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Push, result.GameResult);

            // 21 vs 678 to 21
            judgement = new Judgment(RuleFactory.Build(RuleFactory.BuildAllValidHandPayoutTable()));
            player = MockPlayerStrategy.Build(Card.Six, Card.Seven);
            player.Hit(Card.Eight);
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Six);
            dealer.Hit(Card.Five);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.WinByStraight, result.GameResult);
            // 21 vs 777 too 21
            player = MockPlayerStrategy.Build(Card.Seven, Card.Seven);
            player.Hit(Card.Seven);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.WinByThreeSeven, result.GameResult);
            // 21 vs Ace to Six
            player = MockPlayerStrategy.Build(Card.Ace, Card.Two);
            player.Hit(Card.Three);
            player.Hit(Card.Four);
            player.Hit(Card.Five);
            player.Hit(Card.Six);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.WinByAce2Six, result.GameResult);
            // 21 vs 6under
            player = MockPlayerStrategy.Build(Card.Ace, Card.Two);
            player.Hit(Card.Two);
            player.Hit(Card.Two);
            player.Hit(Card.Three);
            player.Hit(Card.Four);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.WinBySixUnder, result.GameResult);
            // 21 vs 7under
            player.Hit(Card.Three);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.WinBySevenUnder, result.GameResult);
            // 21 vs 7under
            player.Hit(Card.Four);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.WinByEightUnder, result.GameResult);

            // burst vs burst
            player = MockPlayerStrategy.Build(Card.Seven, Card.Seven);
            player.Hit(Card.Eight);
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Six);
            player.Hit(Card.Jack);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Lose, result.GameResult);
            // burst vs 14
            player = MockPlayerStrategy.Build(Card.Seven, Card.Seven);
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Six);
            dealer.Hit(Card.Jack);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Win, result.GameResult);
            // 18 vs 19
            player = MockPlayerStrategy.Build(Card.Seven, Card.Seven);
            player.Hit(Card.Five);
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Six);
            dealer.Hit(Card.Two);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Win, result.GameResult);
            // 19 vs 19
            player = MockPlayerStrategy.Build(Card.Seven, Card.Seven);
            player.Hit(Card.Five);
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.Six);
            dealer.Hit(Card.Three);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Push, result.GameResult);
            // 20 vs 19
            dealer = StrategyFactory.BuildDealer(Card.Jack, Card.King);
            player = MockPlayerStrategy.Build(Card.Ace, Card.Two);
            player.Hit(Card.Six);
            result = judgement.GamePayoutResult(dealer, player);
            Assert.AreEqual(GameResult.Lose, result.GameResult);
        }
    }
}