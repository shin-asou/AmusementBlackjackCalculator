using BlackjackCalculator.Item;
using BlackjackCalculator.Item.Cheat;

namespace BlackjackCalculator.Factory
{
    public static class ShooterFactory
    {
        public static Shooter BuildShooter(int deckCount, int useDeckCount)
        {
            var result = new Shooter(deckCount, useDeckCount);
            result.Build();
            return result;
        }
        public static Shooter BuildCheatShooter(int deckCount, int useDeckCount, List<Card> initCards)
        {
            var result = new CheatShooter(deckCount, useDeckCount, initCards);
            result.Build();
            return result;
        }
    }
}
