namespace BlackjackCalculator.Cards.Tests
{
    [TestClass()]
    public class DeckTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            DeckCreateTest(new Deck(true));
            DeckCreateTest(new Deck(false));
        }
        private static void DeckCreateTest(Deck deck)
        {
            // 総数
            Assert.AreEqual(deck.Count, Deck.MaxCount);

            // 各数値
            foreach (Card.Kind kind in Enum.GetValues(typeof(Card.Kind)))
            {
                Assert.AreEqual(4, deck.Cards.Count(card => card.Type == kind));
            }
            // 各スート
            foreach (Card.SuitType suit in Enum.GetValues(typeof(Card.SuitType)))
            {
                Assert.AreEqual(13, deck.Cards.Count(card => card.Suit == suit));
            }
        }
    }
}