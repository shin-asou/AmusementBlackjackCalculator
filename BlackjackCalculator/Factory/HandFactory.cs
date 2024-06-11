using BlackjackCalculator.Game;
using BlackjackCalculator.Item;

namespace BlackjackCalculator.Factory
{
    public static class HandFactory
    {
        public static Hand Build(Card first, Card second, int splitCount = 0) => new(first, second, splitCount);
    }
}
