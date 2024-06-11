using static BlackjackCalculator.Item.Card;

namespace BlackjackCalculator.Item
{
    public class Card(Card.Kind type, int value, SuitType suit)
    {
        public enum Kind
        {
            Ace,
            King,
            Queen,
            Jack,
            Ten,
            Nine,
            Eight,
            Seven,
            Six,
            Five,
            Four,
            Three,
            Two,
        }
        public enum SuitType
        {
            Spades,
            Hearts,
            Diamonds,
            Clubs,
        }

        public int Value { get; private set; } = value;
        public Kind Type { get; private set; } = type;
        public SuitType Suit { get; private set; } = suit;
        public bool IsAce => Type == Kind.Ace;
        public bool IsTen => Type == Kind.King || Type == Kind.Queen || Type == Kind.Jack || Type == Kind.Ten;

        private static readonly List<Card> allKindNumbers = CreateAllKindNumbers(SuitType.Spades);
        // TODO: できればinternalにしたい(テスト等に使っているがDeckクラスからでもランダムに引けるようにすると解決する)
        public static List<Card> AllKindNumbers { get { return allKindNumbers; } }
        internal static List<Card> CreateAllKindNumbers(SuitType suit)
        {
            var cards = new List<Card>(13)  {
                new (Kind.Ace, 11, suit),
                new (Kind.King, 10, suit),
                new (Kind.Queen, 10, suit),
                new (Kind.Jack, 10, suit),
                new (Kind.Ten, 10, suit),
                new (Kind.Nine, 9, suit),
                new (Kind.Eight, 8, suit),
                new (Kind.Seven, 7, suit),
                new (Kind.Six, 6, suit),
                new (Kind.Five, 5, suit),
                new (Kind.Four, 4, suit),
                new (Kind.Three, 3, suit),
                new (Kind.Two, 2, suit),
            };
            return cards;
        }

        // 特定のシチュエーションを作るため
        public static Card Ace => AllKindNumbers[0];
        public static Card King => AllKindNumbers[1];
        public static Card Queen => AllKindNumbers[2];
        public static Card Jack => AllKindNumbers[3];
        public static Card Ten => AllKindNumbers[4];
        public static Card Nine => AllKindNumbers[5];
        public static Card Eight => AllKindNumbers[6];
        public static Card Seven => AllKindNumbers[7];
        public static Card Six => AllKindNumbers[8];
        public static Card Five => AllKindNumbers[9];
        public static Card Four => AllKindNumbers[10];
        public static Card Three => AllKindNumbers[11];
        public static Card Two => AllKindNumbers[12];
    }
}