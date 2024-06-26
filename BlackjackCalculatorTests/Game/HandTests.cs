﻿using BlackjackCalculator.Factory;
using BlackjackCalculator.Item;

namespace BlackjackCalculator.Game.Tests
{
    [TestClass()]
    public class HandTests
    {
        [TestMethod()]
        public void HandTest()
        {
            Assert.AreEqual(2, CreateHand().Count);
        }

        [TestMethod()]
        public void HitTest()
        {
            var hand = CreateHand();
            hand.Hit(CreateCard());
            Assert.AreEqual(3, hand.Count);
            hand.Hit(CreateCard());
            Assert.AreEqual(4, hand.Count);
        }

        [TestMethod()]
        public void DoubleDownTest()
        {
            var hand = CreateHand();
            hand.DoubleDown(Card.Two);
            Assert.IsTrue(hand.IsDoubleDown);
            Assert.AreEqual(3, hand.Count);
        }

        [TestMethod()]
        public void SplitCountTest()
        {
            var hand = HandFactory.Build(Card.Ace, Card.Ace);
            Assert.AreEqual(0, hand.SplitCount);
            Assert.IsFalse(hand.IsMadeBySplit);
            hand = HandFactory.Build(Card.King, Card.King, 1);
            Assert.AreEqual(1, hand.SplitCount);
            Assert.IsTrue(hand.IsMadeBySplit);
        }
        [TestMethod()]
        public void IsPairHandTest()
        {
            var hand = HandFactory.Build(Card.Ace, Card.Ace);
            Assert.IsTrue(hand.IsPair);
            hand = HandFactory.Build(Card.Eight, Card.Eight);
            Assert.IsTrue(hand.IsPair);
            hand = HandFactory.Build(Card.King, Card.Queen);
            Assert.IsFalse(hand.IsPair);
            hand = HandFactory.Build(Card.Five, Card.Five);
            hand.Hit(Card.Five);
            Assert.IsFalse(hand.IsPair);
        }
        [TestMethod()]
        public void IsSoftHandTest()
        {
            var hand = HandFactory.Build(Card.Ace, Card.Nine);
            Assert.IsTrue(hand.IsSoft);
            hand = HandFactory.Build(Card.Eight, Card.Ace);
            Assert.IsTrue(hand.IsSoft);

            hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            Assert.IsTrue(hand.IsSoft);

            // 5 + A + 6
            hand = HandFactory.Build(Card.Five, Card.Ace);
            hand.Hit(Card.Six);
            Assert.IsFalse(hand.IsSoft);
            hand = HandFactory.Build(Card.Five, Card.Three);
            Assert.IsFalse(hand.IsSoft);
        }
        [TestMethod()]
        public void IsBlackjackTest()
        {
            // Blackjack
            var hand = HandFactory.Build(Card.Ace, Card.King);
            Assert.IsTrue(hand.IsBlackjack);
            hand = HandFactory.Build(Card.Jack, Card.Ace);
            Assert.IsTrue(hand.IsBlackjack);

            // Not Blackjack
            hand = HandFactory.Build(Card.Jack, Card.Queen);
            Assert.IsFalse(hand.IsBlackjack);
            Assert.IsFalse(CreateNotBlackjack21().IsBlackjack);
        }
        [TestMethod()]
        public void IsStraightTest()
        {
            var hand = HandFactory.Build(Card.Eight, Card.Six);
            hand.Hit(Card.Seven);
            Assert.IsTrue(hand.IsStraight);
            hand = HandFactory.Build(Card.Eight, Card.Six);
            hand.Hit(Card.Two);
            hand.Hit(Card.Five);
            Assert.IsFalse(hand.IsStraight);
            hand = HandFactory.Build(Card.Nine, Card.Five);
            hand.Hit(Card.Seven);
            Assert.IsFalse(hand.IsStraight);
        }
        [TestMethod()]
        public void IsThreeSevenTest()
        {
            var hand = HandFactory.Build(Card.Seven, Card.Seven);
            hand.Hit(Card.Seven);
            Assert.IsTrue(hand.IsThreeSeven);
            hand = HandFactory.Build(Card.Six, Card.Six);
            hand.Hit(Card.Six);
            Assert.IsFalse(hand.IsStraight);
            hand = HandFactory.Build(Card.Seven, Card.Seven);
            hand.Hit(Card.Two);
            hand.Hit(Card.Five);
            Assert.IsFalse(hand.IsThreeSeven);
        }
        [TestMethod()]
        public void IsSixUnderTest()
        {
            var hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Two);
            hand.Hit(Card.Four);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Two);
            Assert.IsTrue(hand.IsSixUnder);
            hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Two);
            hand.Hit(Card.Four);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Queen);
            Assert.IsFalse(hand.IsSixUnder);
        }
        [TestMethod()]
        public void IsSevenUnderTest()
        {
            var hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Two);
            hand.Hit(Card.Four);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Two);
            hand.Hit(Card.Ace);
            Assert.IsTrue(hand.IsSevenUnder);
            hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Two);
            hand.Hit(Card.Four);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Queen);
            Assert.IsFalse(hand.IsSevenUnder);
        }
        [TestMethod()]
        public void IsEightUnderTest()
        {
            var hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Two);
            hand.Hit(Card.Four);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Two);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            Assert.IsTrue(hand.IsEightUnder);
            hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Two);
            hand.Hit(Card.Four);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Queen);
            Assert.IsFalse(hand.IsEightUnder);
            // 9枚以上は 8Under
            hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Two);
            hand.Hit(Card.Four);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Two);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            Assert.IsTrue(hand.IsEightUnder);
        }
        [TestMethod()]
        public void IsAce2SixTest()
        {
            var hand = HandFactory.Build(Card.Ace, Card.Two);
            hand.Hit(Card.Three);
            hand.Hit(Card.Four);
            hand.Hit(Card.Five);
            hand.Hit(Card.Six);
            Assert.IsTrue(hand.IsAce2Six);
            hand = HandFactory.Build(Card.Six, Card.Five);
            hand.Hit(Card.Four);
            hand.Hit(Card.Three);
            hand.Hit(Card.Two);
            hand.Hit(Card.Ace);
            Assert.IsTrue(hand.IsAce2Six);
        }
        [TestMethod()]
        public void IsDealerStandTest()
        {
            // 17 or over
            var hand = HandFactory.Build(Card.Eight, Card.Nine);
            Assert.IsTrue(hand.IsDealerStand);
            hand = HandFactory.Build(Card.Jack, Card.Nine);
            Assert.IsTrue(hand.IsDealerStand);
            hand = HandFactory.Build(Card.Two, Card.Six);
            hand.Hit(Card.Queen);
            Assert.IsTrue(hand.IsDealerStand);
            hand = HandFactory.Build(Card.Ace, Card.Six);
            Assert.IsTrue(hand.IsDealerStand);
            hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            Assert.IsTrue(hand.IsDealerStand);

            // 17 under 
            hand = HandFactory.Build(Card.Nine, Card.Seven);
            Assert.IsFalse(hand.IsDealerStand);
            hand = HandFactory.Build(Card.Queen, Card.Six);
            Assert.IsFalse(hand.IsDealerStand);
            hand = HandFactory.Build(Card.Ace, Card.Five);
            Assert.IsFalse(hand.IsDealerStand);
            hand = HandFactory.Build(Card.Ace, Card.Two);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Two);
            Assert.IsFalse(hand.IsDealerStand);
            hand = HandFactory.Build(Card.Seven, Card.Four);
            hand.Hit(Card.Two);
            Assert.IsFalse(hand.IsDealerStand);
        }
        [TestMethod()]
        public void IsBurstrTest()
        {
            var hand = HandFactory.Build(Card.Eight, Card.Eight);
            hand.Hit(Card.Six);
            Assert.IsTrue(hand.IsBurst);
            hand = HandFactory.Build(Card.Seven, Card.Eight);
            hand.Hit(Card.Six);
            Assert.IsFalse(hand.IsBurst);
            hand = HandFactory.Build(Card.Ace, Card.Ace);
            hand.Hit(Card.Queen);
            Assert.IsFalse(hand.IsBurst);
            hand.Hit(Card.Ten);
            Assert.IsTrue(hand.IsBurst);
        }
        [TestMethod()]
        public void IsSofthandrTest()
        {
            var hand = HandFactory.Build(Card.Nine, Card.Ace);
            Assert.IsTrue(hand.IsSoft);
            hand = HandFactory.Build(Card.Two, Card.Three);
            Assert.IsFalse(hand.IsSoft);
            hand = HandFactory.Build(Card.Two, Card.Two);
            Assert.IsFalse(hand.IsSoft);
        }
        [TestMethod()]
        public void ValueTest()
        {
            // Blackjack
            var hand = HandFactory.Build(Card.Ace, Card.King);
            // Assert.Fail();
            Assert.AreEqual(21, hand.Value());

            // 3card 21 is not blackjack
            Assert.AreEqual(21, CreateNotBlackjack21().Value());

            // Picture x2 
            hand = HandFactory.Build(Card.King, Card.Jack);
            Assert.AreEqual(20, hand.Value());
            hand = HandFactory.Build(Card.Queen, Card.Ten);
            Assert.AreEqual(20, hand.Value());

            // A + 6
            hand = HandFactory.Build(Card.Ace, Card.Six);
            Assert.AreEqual(17, hand.Value());

            // A + 5 + A
            hand = HandFactory.Build(Card.Ace, Card.Five);
            Assert.AreEqual(16, hand.Value());
            hand.Hit(Card.Ace);
            Assert.AreEqual(17, hand.Value());

            // A + 3 + J + A + K
            hand = HandFactory.Build(Card.Ace, Card.Three);
            Assert.AreEqual(14, hand.Value());
            hand.Hit(Card.Jack);
            Assert.AreEqual(14, hand.Value());
            hand.Hit(Card.Ace);
            Assert.AreEqual(15, hand.Value());
            hand.Hit(Card.King);
            Assert.AreEqual(25, hand.Value());
        }
        [TestMethod()]
        public void SofthandPairValueTest()
        {
            var hand = HandFactory.Build(Card.Ace, Card.Nine);
            Assert.IsTrue(hand.IsSoft);
            Assert.AreEqual(9, hand.SoftHandPairValue());

            // 2 + 3 + A + A
            hand = HandFactory.Build(Card.Two, Card.Three);
            hand.Hit(Card.Ace);
            hand.Hit(Card.Ace);
            Assert.IsTrue(hand.IsSoft);
            Assert.AreEqual(6, hand.SoftHandPairValue());

            // exception
            hand = HandFactory.Build(Card.Three, Card.Three);
            Assert.IsFalse(hand.IsSoft);
            Assert.ThrowsException<InvalidOperationException>(() => hand.SoftHandPairValue());
        }

        //----------- Test Helper

        private static Hand CreateNotBlackjack21()
        {
            var hand = HandFactory.Build(Card.Five, Card.Five);
            hand.Hit(Card.Ace);
            return hand;
        }
        private static Hand CreateHand()
        {
            return HandFactory.Build(CreateCard(), CreateCard());
        }
        private static Card CreateCard()
        {
            var random = new Random();
            return Card.AllKindNumbers[random.Next(12)];
        }
    }
}