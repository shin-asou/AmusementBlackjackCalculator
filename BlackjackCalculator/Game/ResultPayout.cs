namespace BlackjackCalculator.Game
{
    public readonly struct ResultPayout(bool valid, decimal multiplier, decimal bonus)
    {
        public bool Valid { get; } = valid;
        // payoutは元手もありの入力とする　(等倍配当を2.0とみなす)
        public decimal Multiplier { get; } = multiplier;
        public decimal Bonus { get; } = bonus;
    }
}
