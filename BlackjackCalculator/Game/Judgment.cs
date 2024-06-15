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
        private static void UpCardActionPlayers(DealerStrategy dealer, List<PlayerStrategy> players, PlayerPreActionDelegate actionMethod)
        {
            players.ForEach(player => actionMethod(dealer, player));
        }

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

        public BoxPayout GamePayoutResult(DealerStrategy dealer, PlayerStrategy player)
        {
            return dealer.IsBlackjack ?
                GamePayoutResultDealerBlackjack(dealer, player) : GamePayoutResultDealerNoBlackjack(dealer, player);
        }
        private BoxPayout GamePayoutResultDealerNoBlackjack(DealerStrategy dealer, PlayerStrategy player)
        {
            var handResult = player.Result();
            var gameResult = handResult switch
            {
                HandResult.Blackjack => JudgeValidSpecialHand(GameResult.WinByBlackjack, dealer, player),
                HandResult.SixUnder => JudgeValidSpecialHand(GameResult.WinBySixUnder, dealer, player),
                HandResult.SevenUnder => JudgeValidSpecialHand(GameResult.WinBySevenUnder, dealer, player),
                HandResult.EightUnder => JudgeValidSpecialHand(GameResult.WinByEightUnder, dealer, player),
                HandResult.Straight => JudgeValidSpecialHand(GameResult.WinByStraight, dealer, player),
                HandResult.ThreeSeven => JudgeValidSpecialHand(GameResult.WinByThreeSeven, dealer, player),
                HandResult.Ace2Six => JudgeValidSpecialHand(GameResult.WinByAce2Six, dealer, player),
                _ => JudgeValueOnly(dealer, player),
            };
            var result = new BoxPayout(Rule, dealer, player, gameResult);
            return result;
        }

        private GameResult JudgeValidSpecialHand(GameResult gameResult, DealerStrategy dealer, PlayerStrategy player)
        {
            return Rule.ResultPayout[gameResult].Valid ? gameResult : JudgeValueOnly(dealer, player);
        }

        private static GameResult JudgeValueOnly(DealerStrategy dealer, PlayerStrategy player)
        {
            GameResult gameResult;
            if (player.Result() == HandResult.Burst)
            {
                gameResult = GameResult.Lose;
            }
            else if ((player.Value > dealer.Value) || (dealer.Result() == HandResult.Burst))
            {
                gameResult = GameResult.Win;
            }
            else if (player.Value == dealer.Value)
            {
                gameResult = GameResult.Push;
            }
            else
            {
                gameResult = GameResult.Lose;
            }
            return gameResult;
        }

        // dealer Blackjackはpush evenmoney surrender insualance のパターンがある
        private BoxPayout GamePayoutResultDealerBlackjack(DealerStrategy dealer, PlayerStrategy player)
        {
            GameResult gameResult;
            bool isSuccessInsurance = false;
            if (player.PreActionResult == GamePreAction.No && player.IsBlackjack)
            {
                gameResult = GameResult.Push;
            }
            else if (player.PreActionResult == GamePreAction.EvenMoney && player.IsBlackjack)
            {
                gameResult = GameResult.Win;
            }
            else if (player.PreActionResult == GamePreAction.Surrender)
            {
                gameResult = GameResult.LoseBySurrender;
            }
            else if (player.PreActionResult == GamePreAction.Insurance)
            {
                gameResult = GameResult.Push;
                isSuccessInsurance = true;
            }
            else
            {
                gameResult = GameResult.Lose;
            }
            var result = new BoxPayout(Rule, dealer, player, gameResult)
            {
                IsSuccessInsurance = isSuccessInsurance
            };
            return result;
        }

    }
}
