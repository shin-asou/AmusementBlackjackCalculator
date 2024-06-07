using BlackjackCalculator.Cards;

namespace BlackjackCalculator.Utils
{
    internal static class CardsUtil
    {
        internal static List<Card> Shuffle(List<Card> cards) => [.. cards.OrderBy(c => Guid.NewGuid())];
    }
}
