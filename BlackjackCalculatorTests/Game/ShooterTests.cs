using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackjackCalculator.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackjackCalculator.Factory;
using BlackjackCalculator.Game;
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