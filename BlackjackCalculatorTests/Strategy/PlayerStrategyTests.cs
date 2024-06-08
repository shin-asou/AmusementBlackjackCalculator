using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;
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

        [TestMethod()]
        public void PreActionTest()
        {
            var player = MockPlayerStrategy.Build(Card.Ten, Card.Ace, preAction: GamePreAction.Surrender);
            Assert.AreEqual(GamePreAction.No, player.PreActionResult);
            player.PreAction(Card.Ace);
            Assert.AreEqual(GamePreAction.Surrender, player.PreActionResult);
        }

        [TestMethod()]
        public void ResultTest()
        {
            // Blackjack
            var player = MockPlayerStrategy.Build(Card.Ten, Card.Ace);
            Assert.AreEqual(HandResult.Blackjack, player.Result());
            // value test
            player = MockPlayerStrategy.Build(Card.Ten, Card.Seven);
            Assert.AreEqual(HandResult.Value, player.Result());
            // 22
            player.Hit(Card.Five);
            Assert.AreEqual(HandResult.Burst, player.Result());
            // 21 6x7x8
            player = MockPlayerStrategy.Build(Card.Six, Card.Seven);
            player.Hit(Card.Eight);
            Assert.AreEqual(HandResult.Value, player.Result());
        }

        [TestMethod()]
        public void ActionTest()
        {
            var player = MockPlayerStrategy.Build(Card.Ace, Card.Ace, pairAction: HandAction.Split);
            Assert.AreEqual(HandAction.Split, player.Action(Card.Ace));
            // AA splitは1CardOnlyのルールの場合
            player = MockPlayerStrategy.Build(Card.Ace, Card.Ace, splitCount: 1, pairAction: HandAction.Split, notPairAction: HandAction.Hit);
            Assert.AreEqual(HandAction.Stand, player.Action(Card.Ace));

            // 最大スプリットまでいったら最後はnotPairアクションになる
            var loopCount = 5;
            var lastIndex = loopCount - 1;
            for (int i = 0; i < loopCount; i++)
            {
                player = MockPlayerStrategy.Build(Card.Eight, Card.Eight, splitCount: i, pairAction: HandAction.Split, notPairAction: HandAction.Hit);
                if (i != lastIndex)
                {
                    Assert.AreEqual(HandAction.Split, player.Action(Card.Ace));
                }
                else
                {
                    Assert.AreEqual(HandAction.Hit, player.Action(Card.Ace));
                }
            }

            // ペアハンドだがSplitしない場合
            player = MockPlayerStrategy.Build(Card.Five, Card.Five, pairAction: HandAction.HitOrDoubleDown, notPairAction: HandAction.Stand);
            Assert.AreEqual(HandAction.HitOrDoubleDown, player.Action(Card.Ace));

            player = MockPlayerStrategy.Build(Card.Five, Card.Six, notPairAction: HandAction.HitOrDoubleDown);
            Assert.AreEqual(HandAction.HitOrDoubleDown, player.Action(Card.Ace));
            player = MockPlayerStrategy.Build(Card.Jack, Card.Six, notPairAction: HandAction.Hit);
            Assert.AreEqual(HandAction.Hit, player.Action(Card.Ace));

        }
    }
}