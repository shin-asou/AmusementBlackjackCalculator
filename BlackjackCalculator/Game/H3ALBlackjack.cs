using BlackjackCalculator.Factory;
using BlackjackCalculator.Item;
using BlackjackCalculator.Strategy;

namespace BlackjackCalculator.Game
{
    public class H3ALBlackjack(RuleSet rule) : Blackjack(rule)
    {
        protected override PlayerStrategy CreatePlayerStrategy(Card firstCard, Card secondCard, int splitCount = 0) => StrategyFactory.BuildTrueH3AL(firstCard, secondCard, Rule, splitCount: splitCount);
    }
}
