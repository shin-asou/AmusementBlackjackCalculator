namespace BlackjackCalculator.Game
{
    public readonly record struct ResultPayout(bool Valid, decimal Multiplier, decimal Bonus)
    {
        // payoutMultiplierは元手もありの入力とする　(等倍配当を2.0とみなす)
    }
}
