using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class KnockOutStage
    {
        public QuarterFinalStage QuarterFinals { get; private set; }
        public SemiFinalStage SemiFinals { get; private set; }
        public FinalStage Final { get; private set; }

        public KnockOutStage(List<Team> qualifiedTeams)
        {
            if (qualifiedTeams.Count != 8)
                throw new ArgumentException("There must be exactly 8 qualified teams.");

            var pots = DivideIntoPots(qualifiedTeams);
            QuarterFinals = CreateQuarterFinals(pots);
            SemiFinals = CreateSemiFinals();
            Final = CreateFinal();
        }

        private List<List<Team>> DivideIntoPots(List<Team> teams)
        {
            var potD = teams.Where(t => t.Rank == 1 || t.Rank == 2).ToList();
            var potE = teams.Where(t => t.Rank == 3 || t.Rank == 4).ToList();
            var potF = teams.Where(t => t.Rank == 5 || t.Rank == 6).ToList();
            var potG = teams.Where(t => t.Rank == 7 || t.Rank == 8).ToList();

            return new List<List<Team>> { potD, potE, potF, potG };
        }
        private QuarterFinalStage CreateQuarterFinals(List<List<Team>> pots)
        {
            var quarterFinalMatches = new List<Match>();

            quarterFinalMatches.AddRange(CreateMatches(pots[0], pots[3]));
            quarterFinalMatches.AddRange(CreateMatches(pots[1], pots[2]));

            return new QuarterFinalStage(quarterFinalMatches);
        }
        private SemiFinalStage CreateSemiFinals()
        {
            var semiFinalMatches = new List<Match>
            {
                new Match(null, null),
                new Match(null, null)
            };

            return new SemiFinalStage(semiFinalMatches);
        }

        private FinalStage CreateFinal()
        {
            return new FinalStage(new Match(null, null), new Match(null, null));
        }

        private List<Match> CreateMatches(List<Team> pot1, List<Team> pot2)
        {
            var matches = new List<Match>();
            var random = new Random();

            while (pot1.Count > 0 && pot2.Count > 0)
            {
                var team1 = pot1[random.Next(pot1.Count)];
                var potentialOpponents = pot2.Where(t => !team1.H2HMatches.Any(h2h => !h2h.IsExibitionMatch && h2h.Opponent == t)).ToList();

                if (potentialOpponents.Count == 0)
                {
                    potentialOpponents = pot2;
                }

                var team2 = potentialOpponents[random.Next(potentialOpponents.Count)];

                pot1.Remove(team1);
                pot2.Remove(team2);

                matches.Add(new Match(team1, team2));
            }

            return matches;
        }

        public void SimulateKnockoutStage()
        {
            foreach (var match in QuarterFinals.Matches)
            {
                match.SimulateMatch();
            }

            SemiFinals.Matches[0] = new Match(QuarterFinals.Matches[0].GetWinner(), QuarterFinals.Matches[1].GetWinner());
            SemiFinals.Matches[1] = new Match(QuarterFinals.Matches[2].GetWinner(), QuarterFinals.Matches[3].GetWinner());

            foreach (var match in SemiFinals.Matches)
            {
                match.SimulateMatch();
            }

            var thirdPlaceMatch = new Match(SemiFinals.Matches[0].GetLoser(), SemiFinals.Matches[1].GetLoser());
            Final = new FinalStage(new Match(SemiFinals.Matches[0].GetWinner(), SemiFinals.Matches[1].GetWinner()), thirdPlaceMatch);

            Final.ThirdPlaceMatch.SimulateMatch();
            Final.FinalMatch.SimulateMatch();
        }

        public void DisplayKnockoutResults()
        {
            Console.WriteLine("\nČetvrtfinale:");
            foreach (var match in QuarterFinals.Matches)
            {
                Console.WriteLine($"    {match.Team1.Name} - {match.Team2.Name} ({match.Team1Score}: {match.Team2Score})");
            }

            Console.WriteLine("\nPolufinale:");
            foreach (var match in SemiFinals.Matches)
            {
                Console.WriteLine($"    {match.Team1.Name} - {match.Team2.Name} ({match.Team1Score}: {match.Team2Score})");
            }

            Console.WriteLine("\nUtakmica za treće mesto:");
            Console.WriteLine($"    {Final.ThirdPlaceMatch.Team1.Name} - {Final.ThirdPlaceMatch.Team2.Name} ({Final.ThirdPlaceMatch.Team1Score}: {Final.ThirdPlaceMatch.Team2Score})");

            Console.WriteLine("\nFinale:");
            Console.WriteLine($"    {Final.FinalMatch.Team1.Name} - {Final.FinalMatch.Team2.Name} ({Final.FinalMatch.Team1Score}: {Final.FinalMatch.Team2Score})");

            Console.WriteLine("\nMedalje:");
            Console.WriteLine($"    1. {Final.FinalMatch.GetWinner().Name}");
            Console.WriteLine($"    2. {Final.FinalMatch.GetLoser().Name}");
            Console.WriteLine($"    3. {Final.ThirdPlaceMatch.GetWinner().Name}");
        }
    }
}
