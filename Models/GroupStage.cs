using CodeBehind.BasketBallSimulator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class GroupStage
    {
        public List<Group> Groups { get; set; }
        private const string _dataPath = "Data/groups.json";

        public GroupStage()
        {
            Groups = new List<Group>();
            LoadGroups(_dataPath);
            LoadExhibitionMatches();
        }

        public void LoadExhibitionMatches()
        {
            var allTeams = Groups.SelectMany(g => g.Teams).ToList();
            JsonLoader.LoadExhibitionMatches("Data/exibitions.json", allTeams);
        }


        private void LoadGroups(string relativeFilePath)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string dataFilePath = Path.Combine(currentDirectory, relativeFilePath);

            var groupData = JsonLoader.LoadGroups(dataFilePath);

            if (groupData == null) return;

            foreach (var groupName in groupData.Keys)
            {
                var group = new Group(groupName);
                foreach (var team in groupData[groupName])
                {
                    group.AddTeam(team);
                }
                group.CreateRounds();

                Groups.Add(group);
            }
        }

        public void SimulateGroupStage()
        {
            foreach (var group in Groups)
            {
                group.SimulateGroupStage();
                group.RankTeams();
            }
        }

        public List<Team> GetTeamsForKnockOutPhaseDraw()
        {
            var firstPlaceTeams = Groups.Select(g => g.Teams.First(t => t.Rank == 1)).ToList();
            var secondPlaceTeams = Groups.Select(g => g.Teams.First(t => t.Rank == 2)).ToList();
            var thirdPlaceTeams = Groups.Select(g => g.Teams.First(t => t.Rank == 3)).ToList();

            var rankedFirstPlaceTeams = RankTeamsAcrossGroups(firstPlaceTeams);
            AssignRanks(rankedFirstPlaceTeams, 1);

            var rankedSecondPlaceTeams = RankTeamsAcrossGroups(secondPlaceTeams);
            AssignRanks(rankedSecondPlaceTeams, 4);

            var rankedThirdPlaceTeams = RankTeamsAcrossGroups(thirdPlaceTeams);
            AssignRanks(rankedThirdPlaceTeams, 7);

            var allRankedTeams = new List<Team>();
            allRankedTeams.AddRange(rankedFirstPlaceTeams);
            allRankedTeams.AddRange(rankedSecondPlaceTeams);
            allRankedTeams.AddRange(rankedThirdPlaceTeams.GetRange(0, rankedThirdPlaceTeams.Count - 1));

            return allRankedTeams;
        }

        private List<Team> RankTeamsAcrossGroups(List<Team> teams)
        {
            return teams
                .OrderByDescending(t => t.MatchPoints)
                .ThenByDescending(t => t.GetPointDifference())
                .ThenByDescending(t => t.TotalScoredPoints)
                .ToList();
        }

        private void AssignRanks(List<Team> teams, int startingRank)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].Rank = startingRank + i;
            }
        }

        public void DisplayGroupsStandings()
        {
            foreach (var group in Groups)
            {
                group.DisplayGroupStandings();
            }
        }

        public void DisplayRoundMatches()
        {
            for (int round = 1; round <= 3; round++)
            {
                Console.WriteLine($"Grupna faza - {round}. kolo:");

                foreach (var group in Groups)
                {
                    Console.WriteLine($"    Grupa {group.Name}:");

                    if (!group.Rounds.ContainsKey(round))
                    {
                        continue;
                    }

                    foreach (var match in group.Rounds[round])
                    {
                        Console.WriteLine($"        {match.Team1.Name} - {match.Team2.Name} ({match.Team1Score}:{match.Team2Score})");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
