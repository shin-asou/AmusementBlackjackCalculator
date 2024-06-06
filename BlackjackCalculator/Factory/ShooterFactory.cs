using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackjackCalculator.Game;

namespace BlackjackCalculator.Factory
{
    public static class ShooterFactory
    {
        public static Shooter BuildShooter(int deckCount, int useDeckCount) => new (deckCount, useDeckCount);
    }
}
