using BlackjackCalculator.Cards;
using BlackjackCalculator.Utils;
using System.Runtime.InteropServices;

namespace BlackjackCalculator.Game
{
    // Deckシューター
    public class Shooter
    {
        public Shooter(int deckCount, int endDeckCount)
        {
            DeckCount = deckCount;
            EndDeckCount = endDeckCount;
            Cards = Create();
            if (DeckCount <= EndDeckCount) throw new ArgumentException("deckCount > endDeckCount");
        }

        public Card Pull()
        {
            var result = Cards.First();
            Cards.RemoveAt(0);
            return result;
        }

        public int Count => Cards.Count;
        private List<Card> Cards { get; }
        private int DeckCount { get; }
        private int EndDeckCount { get; }
        private List<Card> Create()
        {
            var result = new List<Card>();
            for (int i = 0; i < DeckCount; i++)
            {
                result.AddRange(new Deck().Cards);
                result = CardsUtil.Shuffle(result);
            }
            return result;
        }
    }
}
