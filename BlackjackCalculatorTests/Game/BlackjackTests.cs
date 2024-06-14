using BlackjackCalculator.Factory;
using BlackjackCalculator.Item;
using BlackjackCalculator.Strategy;
using BlackjackCalculatorTests.Strategy.Mock;

namespace BlackjackCalculator.Game.Tests
{
    [TestClass()]
    public class BlackjackTests
    {
        [TestMethod()]
        public void FirstDealCardPullTest()
        {
            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildShooter(rule.DeckCount, rule.EndDeckCount);
            var boxCount = 4;
            var cards = Blackjack.FirstDealCardPull(shooter, boxCount);
            Assert.AreEqual((boxCount + 1) * 2, cards.Count);
        }

        [TestMethod()]
        public void FirstDealTest()
        {
            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildShooter(rule.DeckCount, rule.EndDeckCount);
            var boxCount = 4;
            var strategies = Blackjack.FirstDeal(rule, shooter, boxCount);
            Assert.AreEqual((boxCount + 1), strategies.Count);
            // 先頭はDealerStrategyで残りはPlayerStrategyである(もしくはその継承クラス)
            for (int i = 0; i < strategies.Count; i++)
            {
                var strategy = strategies[i];
                Assert.IsTrue((i == 0) ? strategy is DealerStrategy : strategy is PlayerStrategy);
            }
        }

        [TestMethod()]
        public void PlayerActionTest()
        {
            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildShooter(rule.DeckCount, rule.EndDeckCount);
            var player = MockPlayerStrategy.Build(Card.Ten, Card.Queen);
            Assert.AreEqual(HandAction.Stand, Blackjack.PlayerAction(rule, player, Card.Ten, shooter));
            // BJ => stand
            player = MockPlayerStrategy.Build(Card.Ten, Card.Ace, notPairAction: HandAction.Hit);
            Assert.AreEqual(HandAction.Stand, Blackjack.PlayerAction(rule, player, Card.Ten, shooter));

            player = MockPlayerStrategy.Build(Card.Nine, Card.Five, notPairAction: HandAction.Hit);
            player.Hit(Card.Queen);
            Assert.AreEqual(HandAction.Stand, Blackjack.PlayerAction(rule, player, Card.Ten, shooter));
        }

        [TestMethod()]
        public void PlayersActionTest()
        {
            // PlayersAction(RuleSet rule, DealerStrategy dealer, List<PlayerStrategy> players, Shooter shooter)
            // PlayersActionのテストを書くためにshooterに細工が必要なのでCheatShooterを作る
            List<Card> cards = [
                Card.Eight, Card.Nine, // dealer 
                Card.Ten, Card.Ace, // player
                Card.Eight, Card.Eight, // player
                Card.Ace, Card.Ace, // player
                Card.Nine, Card.Ten, // player
                Card.Eight, // split one 3
                Card.Eight, // split two 4
                Card.Eight, // can't split
                Card.Four, // hit
                Card.King, // 2
                Card.Queen, // 3
                Card.Nine, // 4
                Card.Ace, // cat't resplit Aces
                ];

            var rule = RuleFactory.BuildBasicRule();
            var shooter = ShooterFactory.BuildCheatShooter(rule.DeckCount, rule.EndDeckCount, cards);
            var dealer = StrategyFactory.BuildDealer(shooter.Pull(), shooter.Pull());
            List<PlayerStrategy> players = [];
            players.Add(StrategyFactory.BuildBasic(shooter.Pull(), shooter.Pull()));
            players.Add(StrategyFactory.BuildBasic(shooter.Pull(), shooter.Pull()));
            players.Add(StrategyFactory.BuildBasic(shooter.Pull(), shooter.Pull()));
            players.Add(StrategyFactory.BuildBasic(shooter.Pull(), shooter.Pull()));

            var result = Blackjack.PlayersAction(rule, dealer, players, shooter);
            Assert.AreEqual(8, result.Count);
        }
    }
}