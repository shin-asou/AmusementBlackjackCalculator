using BlackjackCalculator.Item;

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
    }
}
