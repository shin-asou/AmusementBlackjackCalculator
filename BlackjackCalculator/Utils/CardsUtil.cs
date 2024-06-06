using BlackjackCalculator.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackCalculator.Utils
{
    internal static class CardsUtil
    {
        internal static List<Card> Shuffle(List<Card> cards)
        {
            return [.. cards.OrderBy(c => Guid.NewGuid())];
        }
    }
}
