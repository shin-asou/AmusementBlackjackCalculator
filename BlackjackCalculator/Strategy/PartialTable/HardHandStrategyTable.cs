using BlackjackCalculator.Game;
using System.Collections.Frozen;

namespace BlackjackCalculator.Strategy
{
    public partial class BasicStrategy
    {
        // upCard, TotalValue
        private static readonly FrozenDictionary<int, FrozenDictionary<int, HandAction>> hardHandStrategy = BuildHardHandStrategy();
        // ベーシックストラテジーでは8以下すべてHit、17以上はすべてStandの為Clampで丸める
        // TODO: テスト忘れないように
        private static int ClampHardHandValue(int value) => Math.Clamp(value, 8, 17);

        private static FrozenDictionary<int, FrozenDictionary<int, HandAction>> BuildHardHandStrategy()
        {
            var result = new Dictionary<int, FrozenDictionary<int, HandAction>>
            {
                { 11, BuildHardHandStrategy11() },
                { 10, BuildHardHandStrategy10() },
                { 9, BuildHardHandStrategy9() },
                { 8, BuildHardHandStrategy7to8() },
                { 7, BuildHardHandStrategy7to8() },
                { 6, BuildHardHandStrategy4to6() },
                { 5, BuildHardHandStrategy4to6() },
                { 4, BuildHardHandStrategy4to6() },
                { 3, BuildHardHandStrategy3() },
                { 2, BuildHardHandStrategy2() }
            };
            return result.ToFrozenDictionary();
        }
        // upCard 11 Ace
        private static FrozenDictionary<int, HandAction> BuildHardHandStrategy11()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.Hit },
                { 11, HandAction.Hit },
                { 12, HandAction.Hit },
                { 13, HandAction.Hit },
                { 14, HandAction.Hit },
                { 15, HandAction.Hit },
                { 16, HandAction.HitOrSurrender },
                { 17, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 10 Picture + 10
        private static FrozenDictionary<int, HandAction> BuildHardHandStrategy10()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.Hit },
                { 11, HandAction.HitOrDoubleDown },
                { 12, HandAction.Hit },
                { 13, HandAction.Hit },
                { 14, HandAction.Hit },
                { 15, HandAction.HitOrSurrender },
                { 16, HandAction.HitOrSurrender },
                { 17, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 9
        private static FrozenDictionary<int, HandAction> BuildHardHandStrategy9()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.HitOrDoubleDown },
                { 11, HandAction.HitOrDoubleDown },
                { 12, HandAction.Hit },
                { 13, HandAction.Hit },
                { 14, HandAction.Hit },
                { 15, HandAction.Hit },
                { 16, HandAction.HitOrSurrender },
                { 17, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 7-8
        private static FrozenDictionary<int, HandAction> BuildHardHandStrategy7to8()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.HitOrDoubleDown },
                { 11, HandAction.HitOrDoubleDown },
                { 12, HandAction.Hit },
                { 13, HandAction.Hit },
                { 14, HandAction.Hit },
                { 15, HandAction.Hit },
                { 16, HandAction.Hit },
                { 17, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 4-6
        private static FrozenDictionary<int, HandAction> BuildHardHandStrategy4to6()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.HitOrDoubleDown },
                { 11, HandAction.HitOrDoubleDown },
                { 12, HandAction.Hit },
                { 13, HandAction.Hit },
                { 14, HandAction.Hit },
                { 15, HandAction.Hit },
                { 16, HandAction.Hit },
                { 17, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 3
        private static FrozenDictionary<int, HandAction> BuildHardHandStrategy3()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 8, HandAction.Hit },
                { 9, HandAction.HitOrDoubleDown },
                { 10, HandAction.HitOrDoubleDown },
                { 11, HandAction.HitOrDoubleDown },
                { 12, HandAction.Hit },
                { 13, HandAction.Stand },
                { 14, HandAction.Stand },
                { 15, HandAction.Stand },
                { 16, HandAction.Stand },
                { 17, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 2
        private static FrozenDictionary<int, HandAction> BuildHardHandStrategy2()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.HitOrDoubleDown },
                { 11, HandAction.HitOrDoubleDown },
                { 12, HandAction.Hit },
                { 13, HandAction.Stand },
                { 14, HandAction.Stand },
                { 15, HandAction.Stand },
                { 16, HandAction.Stand },
                { 17, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
    }
}
