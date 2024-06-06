using BlackjackCalculator.Cards;

namespace BlackjackCalculator.Utils
{
    internal static class CardsUtil
    {
        internal static List<Card> Shuffle(List<Card> cards)
        {
            return [.. cards.OrderBy(c => Guid.NewGuid())];
        }
    }
}
