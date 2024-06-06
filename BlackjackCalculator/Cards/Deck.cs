namespace BlackjackCalculator.Cards
{
    public class Deck
    {
        public Deck(bool isShuffle = true)
        {
            var cards = Create();
            Cards = isShuffle ? Shuffle(cards) : cards;
        }

        public List<Card> Cards { get; }
        public int Count { get { return Cards.Count; } }

        private static List<Card> Create()
        {
            var result = new List<Card>();
            // suit4つ分を追加
            foreach (Card.SuitType suit in Enum.GetValues(typeof(Card.SuitType)))
            {
                result.AddRange(Card.CreateAllKindNumbers(suit));
            }
            return result;
        }

        // TODO: シャッフルの責任を持つ適切なクラスは？
        private static List<Card> Shuffle(List<Card> deck)
        {
            return [.. deck.OrderBy(c => Guid.NewGuid())];
        }
    }
}
