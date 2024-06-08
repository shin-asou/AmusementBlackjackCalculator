using BlackjackCalculator.Cards;
using BlackjackCalculator.Factory;
namespace BlackjackCalculator.Game.Tests
{
    [TestClass()]
    public class ShooterTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            var shooter = ShooterFactory.BuildShooter(4, 1);
            Assert.AreEqual(Deck.MaxCount * 4, shooter.Count);
            Assert.ThrowsException<ArgumentException>(() => ShooterFactory.BuildShooter(4, 4));
            Assert.ThrowsException<ArgumentException>(() => ShooterFactory.BuildShooter(4, 5));
        }
        [TestMethod()]
        public void PullTest()
        {
            var shooter = ShooterFactory.BuildShooter(2, 1);
            var startCount = shooter.Count;
            var beforeCard = Card.Ace;
            for (int i = 0; i < 50; i++)
            {
                var result = shooter.Pull();
                var nowCount = shooter.Count;
                Assert.IsNotNull(result);
                Assert.AreEqual(i + 1, startCount - nowCount);
                if (i != 0) { Assert.IsFalse(Card.ReferenceEquals(result, beforeCard)); }
                beforeCard = result;
            }
        }
        [TestMethod()]
        public void IsEndGameTest()
        {
            int deckCount = 6;
            var shooter = ShooterFactory.BuildShooter(deckCount, 1);
            for (int i = 0; i < (Deck.MaxCount * (deckCount - 1)); i++)
            {
                Assert.IsFalse(shooter.IsEndGame);
                shooter.Pull();
            }
            for (int i = 0; i < Deck.MaxCount; i++)
            {
                Assert.IsTrue(shooter.IsEndGame);
                shooter.Pull();
            }
            Assert.AreEqual(0, shooter.Count);
        }
    }
}