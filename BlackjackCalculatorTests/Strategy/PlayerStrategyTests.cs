using BlackjackCalculator.Game;
using BlackjackCalculator.Item;
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

        [TestMethod()]
        public void CanNotHitTest()
        {
            var player = MockPlayerStrategy.Build(Card.Ace, Card.Jack);
            Assert.IsTrue(player.CanNotHit());
            // 20
            player = MockPlayerStrategy.Build(Card.Ten, Card.Jack);
            Assert.IsFalse(player.CanNotHit());
            // AcesSplit 1Card Only
            player = MockPlayerStrategy.Build(Card.Ace, Card.Nine, splitCount: 1);
            Assert.IsTrue(player.CanNotHit());
            // A+9
            player = MockPlayerStrategy.Build(Card.Ace, Card.Nine);
            Assert.IsFalse(player.CanNotHit());
            // 22
            player = MockPlayerStrategy.Build(Card.Ten, Card.Nine);
            player.Hit(Card.Three);
            Assert.IsTrue(player.CanNotHit());
            // doubledown 
            player = MockPlayerStrategy.Build(Card.Two, Card.Nine);
            player.DoubleDown(Card.Two);
            Assert.IsTrue(player.CanNotHit());
            // 3 count
            player = MockPlayerStrategy.Build(Card.Five, Card.Nine);
            player.Hit(Card.Three);
            Assert.IsFalse(player.CanNotHit());
        }

        [TestMethod()]
        public void CanDoubleDownTest()
        {
            // TODO: doubledownTypeとSplitAfterDouble無しのテストを書いていない
            var player = MockPlayerStrategy.Build(Card.Five, Card.Six);
            Assert.IsTrue(player.CanDoubleDown());
            player = MockPlayerStrategy.Build(Card.Ace, Card.Ten);
            Assert.IsFalse(player.CanDoubleDown());
            player = MockPlayerStrategy.Build(Card.Two, Card.Six);
            player.Hit(Card.Two);
            Assert.IsFalse(player.CanDoubleDown());
            // split after doubledown 
            player = MockPlayerStrategy.Build(Card.Six, Card.Two, splitCount: 1);
            Assert.IsTrue(player.CanDoubleDown());
        }
    }
}