using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class Match
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }

        public Match(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
        }

        public void SimulateMatch()
        {
            GenerateScores();
            UpdateTeamsStatistics();
        }

        private void GenerateScores()
        {
            double probabilityA = CalculateWinningProbability(Team1.FIBARanking, Team2.FIBARanking);
            double probabilityB = 1 - probabilityA;

            double formFactorA = Team1.CalculateFormFactor();
            double formFactorB = Team2.CalculateFormFactor();

            Random random = new Random();
            int baseScore = 60;

            Team1Score = (int)(baseScore + random.Next(0, 60) * probabilityA * formFactorA);
            Team2Score = (int)(baseScore + random.Next(0, 60) * probabilityB * formFactorB);

            Team1Score = Math.Min(Team1Score, 120);
            Team2Score = Math.Min(Team2Score, 120);
        }

        private void UpdateTeamsStatistics()
        {
            if (Team1Score > Team2Score)
            {
                Team1.Wins++;
                Team2.Losses++;
                Team1.MatchPoints += 2;
                Team2.MatchPoints += 1;
            }
            else
            {
                Team2.Wins++;
                Team1.Losses++;
                Team2.MatchPoints += 2;
                Team1.MatchPoints += 1;
            }

            Team1.TotalScoredPoints += Team1Score;
            Team1.TotalConcededPoints += Team2Score;
            Team2.TotalScoredPoints += Team2Score;
            Team2.TotalConcededPoints += Team1Score;

            Team1.H2HMatches.Add(new H2H(Team2, Team1Score, Team2Score));
            Team2.H2HMatches.Add(new H2H(Team1, Team2Score, Team1Score));
        }
        private double CalculateWinningProbability(int rankingA, int rankingB)
        {
            double totalRank = rankingA + rankingB;
            return (double)rankingB / totalRank;
        }

        public Team GetWinner()
        {
            return Team1Score > Team2Score ? Team1 : Team2;
        }

        public Team GetLoser()
        {
            return Team1Score < Team2Score ? Team1 : Team2;
        }
    }
}
