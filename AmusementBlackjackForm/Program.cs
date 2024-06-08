using BlackjackCalculator.Game;

namespace AmusementBlackjackForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var game = new Blackjack();
            game.Flow();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new AmusementBlackjackEVCalculator());
        }
    }
}