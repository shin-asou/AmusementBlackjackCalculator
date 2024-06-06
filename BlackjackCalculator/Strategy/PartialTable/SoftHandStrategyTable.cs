using BlackjackCalculator.Game;
using System.Collections.Frozen;

namespace BlackjackCalculator.Strategy
{
    public partial class BasicStrategy
    {
        // upCard, SecondValue
        private static readonly FrozenDictionary<int, FrozenDictionary<int, HandAction>> softHandStrategy = BuildSoftHandStrategy();
        // ベーシックストラテジーでは8以上はすべて為Clampで丸める
        // TODO: テスト忘れないように
        private static int ClampSoftHandValue(int value) => Math.Clamp(value, 1, 8);

        private static FrozenDictionary<int, FrozenDictionary<int, HandAction>> BuildSoftHandStrategy()
        {
            var result = new Dictionary<int, FrozenDictionary<int, HandAction>>
            {
                { 11, BuildSoftHandStrategy9to11() },
                { 10, BuildSoftHandStrategy9to11() },
                { 9, BuildSoftHandStrategy9to11() },
                { 8, BuildSoftHandStrategy7to8() },
                { 7, BuildSoftHandStrategy7to8() },
                { 6, BuildSoftHandStrategy5to6() },
                { 5, BuildSoftHandStrategy5to6() },
                { 4, BuildSoftHandStrategy4() },
                { 3, BuildSoftHandStrategy3() },
                { 2, BuildSoftHandStrategy2() }
            };
            return result.ToFrozenDictionary();
        }

        // upCard 11 to 9
        private static FrozenDictionary<int, HandAction> BuildSoftHandStrategy9to11()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 1, HandAction.Hit },
                { 2, HandAction.Hit },
                { 3, HandAction.Hit },
                { 4, HandAction.Hit },
                { 5, HandAction.Hit },
                { 6, HandAction.Hit },
                { 7, HandAction.Hit },
                { 8, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 7 to 8
        private static FrozenDictionary<int, HandAction> BuildSoftHandStrategy7to8()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 1, HandAction.Hit },
                { 2, HandAction.Hit },
                { 3, HandAction.Hit },
                { 4, HandAction.Hit },
                { 5, HandAction.Hit },
                { 6, HandAction.Hit },
                { 7, HandAction.Stand },
                { 8, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 5 tio 6
        private static FrozenDictionary<int, HandAction> BuildSoftHandStrategy5to6()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 1, HandAction.HitOrDoubleDown },
                { 2, HandAction.HitOrDoubleDown },
                { 3, HandAction.HitOrDoubleDown },
                { 4, HandAction.HitOrDoubleDown },
                { 5, HandAction.HitOrDoubleDown },
                { 6, HandAction.HitOrDoubleDown },
                { 7, HandAction.HitOrDoubleDown },
                { 8, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 4
        private static FrozenDictionary<int, HandAction> BuildSoftHandStrategy4()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 1, HandAction.Hit },
                { 2, HandAction.Hit },
                { 3, HandAction.Hit },
                { 4, HandAction.HitOrDoubleDown },
                { 5, HandAction.HitOrDoubleDown },
                { 6, HandAction.HitOrDoubleDown },
                { 7, HandAction.HitOrDoubleDown },
                { 8, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 3
        private static FrozenDictionary<int, HandAction> BuildSoftHandStrategy3()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 1, HandAction.Hit },
                { 2, HandAction.Hit },
                { 3, HandAction.Hit },
                { 4, HandAction.Hit },
                { 5, HandAction.Hit },
                { 6, HandAction.HitOrDoubleDown },
                { 7, HandAction.HitOrDoubleDown },
                { 8, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // upCard 3
        private static FrozenDictionary<int, HandAction> BuildSoftHandStrategy2()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 1, HandAction.Hit },
                { 2, HandAction.Hit },
                { 3, HandAction.Hit },
                { 4, HandAction.Hit },
                { 5, HandAction.Hit },
                { 6, HandAction.Hit },
                { 7, HandAction.Stand },
                { 8, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
    }
}
