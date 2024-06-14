using static BlackjackCalculator.Item.Card;

namespace BlackjackCalculator.Item
{
    public class BlankCard(Card.Kind type, int value, SuitType suit) : Card(type, value, suit)
    {
        public override bool IsBlank => true;
        public static Card Build() => new BlankCard(Kind.Ace, 11, SuitType.Clubs);
    }
}
