using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;
using BlackjackCalculator.Strategy;

namespace BlackjackCalculator.Factory
{
    public static class StrategyFactory
    {
        public static DealerStrategy BuildDealer(Card first, Card second) => new(HandFactory.Build(first, second));
        public static BasicStrategy BuildBasic(Card first, Card second) => new(HandFactory.Build(first, second));
    }
}
