using BlackjackCalculator.Item;
using static BlackjackCalculator.Item.Card;

namespace BlackjackCalculator.Game
{
    public class Hand(Card first, Card second, int splitCount = 0)
    {
        private List<Card> Cards { get; } = [first, second];
        public int SplitCount { get; } = splitCount;
        public bool IsBlank => Cards.Any(c => c.IsBlank);

        public int Count => Cards.Count;
        public Card FirstCard => Cards[0];
        public Card SecondCard => Cards[1];
        public Card UpCard => FirstCard;
        public bool IsUpCardAce => UpCard.IsAce;
        private bool ExistsAce => Cards.Exists(card => card.IsAce);
        public bool FirstDeal => Cards.Count == 2;

        public bool IsDoubleDown { get; private set; } = false;
        public bool IsMadeBySplit => SplitCount != 0;
        public bool IsMadeByAcesSplit => IsMadeBySplit && FirstCard.IsAce;
        public bool IsPair => FirstDeal && FirstCard.Type == SecondCard.Type;
        // Aceを含みAce1枚を除いた合計が10以下、つまりAceを11として扱った場合でも21以下になる状態をソフトハンドと判定
        public bool IsSoft => ExistsAce && (CalculateValueExcludeSoftHandAce() <= 10);
        public static int DealerStandValue => 17;
        public bool IsSoftDealerStandValue => IsSoft && Value() == DealerStandValue;
        public bool IsDealerStand => Value() >= DealerStandValue;
        public bool IsBlackjack => FirstDeal && Value() == 21;
        public bool IsStraight
        {
            get
            {
                return Count == 3 &&
                    Cards.Any(c => c.Type == Kind.Six) &&
                    Cards.Any(c => c.Type == Kind.Seven) &&
                    Cards.Any(c => c.Type == Kind.Eight);
            }
        }
        public bool IsThreeSeven => Count == 3 && Cards.All(c => c.Type == Kind.Seven);
        public bool IsSixUnder => Count == 6 && !IsBurst;
        public bool IsSevenUnder => Count == 7 && !IsBurst;
        public bool IsEightUnder => Count >= 8 && !IsBurst;
        public bool IsAce2Six => Count == 6 && CountForAceToSix() == 6;
        public bool IsReachAce2Six => Count == 5 && CountForAceToSix() == 5;
        public bool IsSpecial => IsBlackjack || IsStraight || IsThreeSeven || IsSixUnder || IsSevenUnder || IsEightUnder || IsAce2Six;
        public bool IsBurst => Value() > 21;

        private int CountForAceToSix()
        {
            var count = 0;
            if (Cards.Any(c => c.IsAce)) count++;
            if (Cards.Any(c => c.Type == Kind.Two)) count++;
            if (Cards.Any(c => c.Type == Kind.Three)) count++;
            if (Cards.Any(c => c.Type == Kind.Four)) count++;
            if (Cards.Any(c => c.Type == Kind.Five)) count++;
            if (Cards.Any(c => c.Type == Kind.Six)) count++;
            return count;
        }

        public void Hit(Card newCard) => Cards.Add(newCard);
        public void DoubleDown(Card newCard)
        {
            IsDoubleDown = true;
            Hit(newCard);
        }

        // Aceを限界まで1として計算する
        public int MinValue()
        {
            return CalculateValueMinValue();
        }
        public int SoftHandPairValue()
        {
            if (!IsSoft) { throw new InvalidOperationException("this method call prerequisites IsSoft == true"); }
            return CalculateValueExcludeSoftHandAce();
        }
        public int Value()
        {
            return ExistsAce ? CalculateValueIsContainAce() : Cards.Sum(card => card.Value);
        }
        // 手順
        // (CalculateValueExcludeSoftHandAce) Ace以外のすべてのカードの合計を計算 
        // Aceが2枚以上ある場合2枚目以降は必ず1として扱うのですべてのAceの枚数から1を引くことで1or11の計算が必要なカード以外が計算可能 (ただしこの処理より前にAceを含んでいるかをチェックしている前提
        // 最後にAceを1で扱うべきか11で扱うべきかを処理する
        private int CalculateValueIsContainAce()
        {
            var value = CalculateValueExcludeSoftHandAce();
            return value += (IsSoft) ? 11 : 1;
        }
        private int CalculateValueExcludeSoftHandAce()
        {
            var value = Cards.Where(card => !card.IsAce).Sum(card => card.Value);
            return value + (Cards.Count(card => card.IsAce) - 1);
        }
        private int CalculateValueMinValue()
        {
            var value = Cards.Where(card => !card.IsAce).Sum(card => card.Value);
            return value + Cards.Count(card => card.IsAce);
        }
    }
}

