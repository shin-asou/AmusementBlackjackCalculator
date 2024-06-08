using BlackjackCalculator.Strategy;

namespace BlackjackCalculator.Game
{
    public class Judgment(RuleSet rule)
    {
        RuleSet Rule { get; } = rule;
        public void PreActionPlayers(DealerStrategy dealer, List<PlayerStrategy> players)
        {
            UpCardActionPlayers(
                dealer, players,
                dealer.IsUpCardAce ? new PlayerPreActionDelegate(PreActionUpCardAce) : new PlayerPreActionDelegate(PreActionUpCardNotAce)
            );
        }

        private delegate GameResult PlayerPreActionDelegate(DealerStrategy dealer, PlayerStrategy player);
        private static void UpCardActionPlayers(DealerStrategy dealer, List<PlayerStrategy> players, PlayerPreActionDelegate actionMethod) => players.ForEach(player => actionMethod(dealer, player));
        public GameResult PreActionUpCardAce(DealerStrategy dealer, PlayerStrategy player)
        {
            player.PreAction(dealer.UpCard);
            if (player.IsPreActionSurrender) return GameResult.LoseBySurrender;
            if (dealer.IsNoBlackjack) return GameResult.No;
            if (player.IsEvenMoney) return GameResult.Win;
            if (player.IsInsurance) return GameResult.Push;
            if (player.IsBlackjack) return GameResult.Push;
            return GameResult.Lose;
        }
        public GameResult PreActionUpCardNotAce(DealerStrategy dealer, PlayerStrategy player)
        {
            player.PreAction(dealer.UpCard);
            if (player.IsPreActionSurrender) return GameResult.LoseBySurrender;
            if (dealer.IsNoBlackjack) return GameResult.No;
            return player.IsBlackjack ? GameResult.Push : GameResult.Lose;
        }
    }
}
