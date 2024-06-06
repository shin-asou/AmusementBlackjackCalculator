using BlackjackCalculator.Cards;
using BlackjackCalculator.Game;
using BlackjackCalculator.Strategy;

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
            var deck = new Deck();
            var strategy = new BasicStrategy(new Hand(Card.Ten, Card.Nine));
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new AmusementBlackjackEVCalculator());
        }
    }
}