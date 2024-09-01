using CodeBehind.BasketBallSimulator.Models;

namespace CodeBehind.BasketBallSimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tournament = new Tournament();
            tournament.ExecuteTournament();
        }
    }
}
