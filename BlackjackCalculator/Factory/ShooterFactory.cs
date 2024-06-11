using BlackjackCalculator.Item;

namespace BlackjackCalculator.Factory
{
    public static class ShooterFactory
    {
        public static Shooter BuildShooter(int deckCount, int useDeckCount) => new(deckCount, useDeckCount);
    }
}
