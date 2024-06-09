using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;
using BlackjackCalculator.Strategy;

namespace BlackjackCalculator.Factory
{
    public static class StrategyFactory
    {
        public static DealerStrategy BuildDealer(Card first, Card second) => BuildDealer(first, second, RuleFactory.BuildBasicRule());
        public static DealerStrategy BuildDealer(Card first, Card second, RuleSet rule) => new(HandFactory.Build(first, second), rule);
        public static BasicStrategy BuildBasic(Card first, Card second) => BuildBasic(first, second, RuleFactory.BuildBasicRule(), 0);
        public static BasicStrategy BuildBasic(Card first, Card second, RuleSet rule) => BuildBasic(first, second, rule, 0);
        public static BasicStrategy BuildBasic(Card first, Card second, RuleSet rule, int splitCount) => new(HandFactory.Build(first, second, splitCount), rule);

        public static PlayerStrategy BuildNullPlayer(Card first, int splitCount) => new NullPlayerStrategy(HandFactory.Build(first, first, splitCount), RuleFactory.BuildBasicRule());
    }
}
