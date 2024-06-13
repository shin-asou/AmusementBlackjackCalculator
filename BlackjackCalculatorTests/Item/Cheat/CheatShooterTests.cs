using BlackjackCalculator.Factory;

namespace BlackjackCalculator.Item.Cheat.Tests
{
    [TestClass()]
    public class CheatShooterTests
    {
        [TestMethod()]
        public void BuildTest()
        {
            List<Card> cards = [
                Card.Eight, Card.Seven, Card.Six, Card.Five,
                Card.Ten,
                Card.Eight, Card.Seven, Card.Nine, Card.Six,
                Card.Eight, Card.Ace,
                Card.Eight, Card.Eight,
                Card.Five,
                ];
            var cShooter = ShooterFactory.BuildCheatShooter(6, 1, cards);

            foreach (var card in cards)
            {
                var pullCard = cShooter.Pull();
                Assert.AreEqual(card.Type, pullCard.Type);
            }
        }
    }
}