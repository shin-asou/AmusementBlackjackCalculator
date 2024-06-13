using BlackjackCalculator.Utils;

namespace BlackjackCalculator.Item
{
    // Deckシューター
    public class Shooter
    {
        public Shooter(int deckCount, int endDeckCount)
        {
            DeckCount = deckCount;
            EndDeckCount = endDeckCount;
            if (DeckCount <= EndDeckCount) throw new ArgumentException("deckCount > endDeckCount");
        }

        public virtual Card Pull()
        {
            var result = Cards.First();
            Cards.RemoveAt(0);
            return result;
        }

        public int Count => Cards.Count;
        public bool IsEndGame => Count <= EndDeckCount * Deck.MaxCount;
        protected List<Card> Cards { get; set; } = [];
        protected int DeckCount { get; }
        protected int EndDeckCount { get; }

        public virtual void Build() => Cards = CreateCards();
        protected List<Card> CreateCards()
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
