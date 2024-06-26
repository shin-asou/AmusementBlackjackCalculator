﻿using BlackjackCalculator.Game;
using BlackjackCalculator.Item;

namespace BlackjackCalculator.Strategy
{
    public abstract class AbstractStrategy(Hand hand, RuleSet rule)
    {
        protected RuleSet Rule { get; } = rule;
        protected Hand Hand { get; } = hand;
        public bool IsBlank => Hand.IsBlank;
        public bool IsBlackjack => Hand.IsBlackjack;
        public bool IsNoBlackjack => !Hand.IsBlackjack;
        public int Value => Hand.Value();
        public abstract HandAction Action();
        public abstract HandAction Action(Card upCard);
        public abstract HandResult Result();
        public void Hit(Card card) => Hand.Hit(card);
    }
}
