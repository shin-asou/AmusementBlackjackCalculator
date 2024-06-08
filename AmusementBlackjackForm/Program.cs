using BlackjackCalculator.Cards;
using BlackjackCalculator.Factory;
using BlackjackCalculator.Game;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Numerics;

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
            //var game = new Blackjack();
            //game.Flow();
            //var judgement = new Judgment(RuleFactory.BuildBasicRule());
            //var dealer = StrategyFactory.BuildDealer(Card.Ace, Card.Ten);
            //var player = StrategyFactory.BuildBasic(Card.Queen, Card.Ace);
            //GameResult result =  judgement.PreActionUpCardAce(dealer, player);
            // To customize application configuration such as set high DPI settings or default fon
            // t,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new AmusementBlackjackEVCalculator());
        }
    }
}