using BlackjackCalculator.Utils;

namespace BlackjackCalculator.Item
{
    public class Deck
    {
        public const int MaxCount = 52;

        public Deck(bool isShuffle = true)
        {
            var cards = Create();
            Cards = isShuffle ? CardsUtil.Shuffle(cards) : cards;
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
    }
}
