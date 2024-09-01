using CodeBehind.BasketBallSimulator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class Team
    {
        [JsonPropertyName("Team")]
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public int FIBARanking { get; set; }
        public int MatchPoints { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TotalScoredPoints { get; set; }
        public int TotalConcededPoints { get; set; }
        public List<H2H> H2HMatches { get; set; }
        public char Pot { get; set; }
        public int Rank { get; set; }

        public Team(string name, string isoCode, int fibaRanking)
        {
            Name = name;
            ISOCode = isoCode;
            FIBARanking = fibaRanking;
            H2HMatches = new List<H2H>();
        }

        public int GetPointDifference() => TotalScoredPoints - TotalConcededPoints;

        public double CalculateFormFactor()
        {
            var last5Matches = H2HMatches
                .TakeLast(5)
                .ToList();

            double formFactor = 1.0;
            foreach (var match in last5Matches)
            {
                if (match.MatchOutcome == MatchOutcome.WIN)
                {
                    formFactor += 0.2;
                }
                else if (match.MatchOutcome == MatchOutcome.LOSE)
                {
                    formFactor -= 0.1;
                }
            }

            return Math.Max(formFactor, 0.8);
        }
    }
}
