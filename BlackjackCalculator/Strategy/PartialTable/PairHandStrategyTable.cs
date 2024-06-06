using BlackjackCalculator.Game;
using System.Collections.Frozen;

namespace BlackjackCalculator.Strategy
{
    public partial class BasicStrategy
    {
        // pairValue, upCard
        private static readonly FrozenDictionary<int, FrozenDictionary<int, HandAction>> pairHandStrategy = BuildPairHandStrategy();

        private static FrozenDictionary<int, FrozenDictionary<int, HandAction>> BuildPairHandStrategy()
        {
            var result = new Dictionary<int, FrozenDictionary<int, HandAction>>
            {
                { 11, BuildPairHandStrategy11and8() },
                { 10, BuildPairHandStrategy10() },
                { 9, BuildPairHandStrategy9() },
                { 8, BuildPairHandStrategy11and8() },
                { 7, BuildPairHandStrategy7and3to2() },
                { 6, BuildPairHandStrategy6() },
                { 5, BuildPairHandStrategy5() },
                { 4, BuildPairHandStrategy4() },
                { 3, BuildPairHandStrategy7and3to2() },
                { 2, BuildPairHandStrategy7and3to2() },
            };
            return result.ToFrozenDictionary();
        }
        // pair 11 and 8
        private static FrozenDictionary<int, HandAction> BuildPairHandStrategy11and8()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 2, HandAction.Split },
                { 3, HandAction.Split },
                { 4, HandAction.Split },
                { 5, HandAction.Split },
                { 6, HandAction.Split },
                { 7, HandAction.Split },
                { 8, HandAction.Split },
                { 9, HandAction.Split },
                { 10, HandAction.Split },
                { 11, HandAction.Split },
            };
            return result.ToFrozenDictionary();
        }
        // pair 10
        private static FrozenDictionary<int, HandAction> BuildPairHandStrategy10()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 2, HandAction.Stand },
                { 3, HandAction.Stand },
                { 4, HandAction.Stand },
                { 5, HandAction.Stand },
                { 6, HandAction.Stand },
                { 7, HandAction.Stand },
                { 8, HandAction.Stand },
                { 9, HandAction.Stand },
                { 10, HandAction.Stand },
                { 11, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // pair 9
        private static FrozenDictionary<int, HandAction> BuildPairHandStrategy9()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 2, HandAction.Split },
                { 3, HandAction.Split },
                { 4, HandAction.Split },
                { 5, HandAction.Split },
                { 6, HandAction.Split },
                { 7, HandAction.Stand },
                { 8, HandAction.Split },
                { 9, HandAction.Split },
                { 10, HandAction.Stand },
                { 11, HandAction.Stand },
            };
            return result.ToFrozenDictionary();
        }
        // pair 7 and 3 and 2
        private static FrozenDictionary<int, HandAction> BuildPairHandStrategy7and3to2()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 2, HandAction.Split },
                { 3, HandAction.Split },
                { 4, HandAction.Split },
                { 5, HandAction.Split },
                { 6, HandAction.Split },
                { 7, HandAction.Split },
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.Hit },
                { 11, HandAction.Hit },
            };
            return result.ToFrozenDictionary();
        }
        // pair 6
        private static FrozenDictionary<int, HandAction> BuildPairHandStrategy6()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 2, HandAction.Split },
                { 3, HandAction.Split },
                { 4, HandAction.Split },
                { 5, HandAction.Split },
                { 6, HandAction.Split },
                { 7, HandAction.Hit },
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.Hit },
                { 11, HandAction.Hit },
            };
            return result.ToFrozenDictionary();
        }
        // pair 5
        private static FrozenDictionary<int, HandAction> BuildPairHandStrategy5()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 2, HandAction.HitOrDoubleDown },
                { 3, HandAction.HitOrDoubleDown },
                { 4, HandAction.HitOrDoubleDown },
                { 5, HandAction.HitOrDoubleDown },
                { 6, HandAction.HitOrDoubleDown },
                { 7, HandAction.HitOrDoubleDown },
                { 8, HandAction.HitOrDoubleDown },
                { 9, HandAction.HitOrDoubleDown },
                { 10, HandAction.Hit },
                { 11, HandAction.Hit },
            };
            return result.ToFrozenDictionary();
        }
        // pair 4
        private static FrozenDictionary<int, HandAction> BuildPairHandStrategy4()
        {
            var result = new Dictionary<int, HandAction>
            {
                { 2, HandAction.Hit },
                { 3, HandAction.Hit },
                { 4, HandAction.Hit },
                { 5, HandAction.Split },
                { 6, HandAction.Split },
                { 7, HandAction.Hit },
                { 8, HandAction.Hit },
                { 9, HandAction.Hit },
                { 10, HandAction.Hit },
                { 11, HandAction.Hit },
            };
            return result.ToFrozenDictionary();
        }
    }
}
