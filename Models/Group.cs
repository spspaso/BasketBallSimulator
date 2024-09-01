using CodeBehind.BasketBallSimulator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class Group
    {
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
        public List<Match> Matches { get; set; }
        public Dictionary<int, List<Match>> Rounds { get; set; }

        public Group(string name)
        {
            Name = name;
            Teams = new List<Team>();
            Matches = new List<Match>();
            Rounds = new Dictionary<int, List<Match>>();
        }

        public void AddTeam(Team team)
        {
            Teams.Add(team);
        }

        public void CreateRounds()
        {
            if (Teams.Count != 4)
            {
                throw new InvalidOperationException("Each group must have exactly 4 teams.");
            }

            Rounds = new Dictionary<int, List<Match>>
                {
                    { 1, new List<Match>
                        {
                            new Match(Teams[0], Teams[1]),
                            new Match(Teams[2], Teams[3])
                        }
                    },
                    { 2, new List<Match>
                        {
                            new Match(Teams[0], Teams[2]),
                            new Match(Teams[1], Teams[3])
                        }
                    },
                    { 3, new List<Match>
                        {
                            new Match(Teams[0], Teams[3]),
                            new Match(Teams[1], Teams[2])
                        }
                    }
                };
        }

        public void SimulateGroupStage()
        {
            foreach (var round in Rounds)
            {
                foreach (var match in round.Value)
                {
                    match.SimulateMatch();
                }
            }
        }

        public void RankTeams()
        {
            var rankedTeams = Teams
                .OrderByDescending(t => t.MatchPoints)
                .ToList();

            for (int i = 0; i < rankedTeams.Count - 1;)
            {
                var currentTeam = rankedTeams[i];
                var tiedTeams = rankedTeams.Where(t => t.MatchPoints == currentTeam.MatchPoints).ToList();

                if (tiedTeams.Count == 2)
                {
                    RankTwoTeams(tiedTeams[0], tiedTeams[1], rankedTeams);
                    i += 2;
                }
                else if (tiedTeams.Count == 3)
                {
                    RankThreeTeams(tiedTeams, rankedTeams, i);
                    i += 3;
                }
                i++;
            }

            for (int i = 0; i < rankedTeams.Count; i++)
            {
                rankedTeams[i].Rank = i + 1;
            }

            Teams = rankedTeams.OrderBy(t => t.Rank).ToList();
        }

        private void RankTwoTeams(Team team1, Team team2, List<Team> rankedTeams)
        {
            var h2hMatch = team1.H2HMatches.FirstOrDefault(h => !h.IsExibitionMatch && h.Opponent == team2);

            if (h2hMatch == null) return;

            if (h2hMatch.MatchOutcome == MatchOutcome.WIN)
            {
                rankedTeams.Remove(team2);
                rankedTeams.Insert(rankedTeams.IndexOf(team1) + 1, team2);
            }
            else
            {
                rankedTeams.Remove(team1);
                rankedTeams.Insert(rankedTeams.IndexOf(team2) + 1, team1);
            }
        }

        private void RankThreeTeams(List<Team> tiedTeams, List<Team> rankedTeams, int index)
        {
            var pointDifferences = tiedTeams
                .Select(t => new
                {
                    Team = t,
                    PointsDifference = t.H2HMatches
                        .Where(h => tiedTeams.Contains(h.Opponent))
                        .Sum(h => h.Score - h.OpponentScore)
                })
                .OrderByDescending(t => t.PointsDifference)
                .ToList();

            for (int i = 0; i < pointDifferences.Count; i++)
            {
                var team = pointDifferences[i].Team;
                rankedTeams.Remove(team);
                rankedTeams.Insert(index, team);
                index++;
            }
        }

        public void DisplayGroupStandings()
        {
            Console.WriteLine($"Konačan plasman u grupi {Name}:");

            Console.WriteLine("Ime - pobede/porazi/bodovi/postignuti koševi/primljeni koševi/koš razlika:");

            for (int i = 0; i < Teams.Count; i++)
            {
                var team = Teams[i];
                string pointDifference = (team.TotalScoredPoints - team.TotalConcededPoints).ToString("+#;-#;0");
                Console.WriteLine($"{i + 1}. {team.Name,-12} {team.Wins} / {team.Losses} / {team.MatchPoints} / {team.TotalScoredPoints} / {team.TotalConcededPoints} / {pointDifference}");
            }
            Console.WriteLine();
        }

    }
}
