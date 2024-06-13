namespace BlackjackCalculator.Item.Cheat
{
    public class CheatShooter(int deckCount, int endDeckCount, List<Card> initCards) : Shooter(deckCount, endDeckCount)
    {
        private readonly List<Card> _initCards = initCards;
        public override void Build()
        {
            var baseCards = CreateCards();
            for (var i = 0; i < _initCards.Count; i++)
            {
                var card = _initCards[i];
                var replaceIndex = baseCards.FindIndex(i, baseCards.Count - (i + 1), c => c.Type == card.Type);
                (baseCards[i], baseCards[replaceIndex]) = (baseCards[replaceIndex], baseCards[i]);
            }
            Cards = baseCards;
        }
    }
}
