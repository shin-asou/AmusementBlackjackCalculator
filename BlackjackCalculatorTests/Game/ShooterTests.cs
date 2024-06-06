using BlackjackCalculator.Factory;
namespace BlackjackCalculator.Game.Tests
{
    [TestClass()]
    public class ShooterTests
    {
        [TestMethod()]
        public void PullTest()
        {
            var shooter = ShooterFactory.BuildShooter(2, 1);
            var startCount = shooter.Count;
            for (int i = 0; i < 50; i++)
            {
                var result = shooter.Pull();
                var nowCount = shooter.Count;
                Assert.IsNotNull(result);
                Assert.AreEqual(i + 1, startCount - nowCount);
            }
        }
    }
}