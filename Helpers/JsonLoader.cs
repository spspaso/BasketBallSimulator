using CodeBehind.BasketBallSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Helpers
{
    public static class JsonLoader
    {
        public static Dictionary<string, List<Team>> LoadGroups(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, List<Team>>>(jsonData);
        }

        public static void LoadExhibitionMatches(string jsonFilePath, List<Team> teams)
        {

            string currentDirectory = Directory.GetCurrentDirectory();

            string dataFilePath = Path.Combine(currentDirectory, jsonFilePath);

            var jsonData = File.ReadAllText(dataFilePath);
            var exhibitionMatches = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, string>>>>(jsonData);
            if (exhibitionMatches == null) return;

            foreach (var entry in exhibitionMatches)
            {
                string teamIsoCode = entry.Key;
                var matches = entry.Value;

                var team = teams.FirstOrDefault(t => t.ISOCode == teamIsoCode);
                if (team == null) return;

                foreach (var matchData in matches)
                {
                    var opponentCode = matchData["Opponent"];
                    var opponent = teams.FirstOrDefault(t => t.ISOCode == opponentCode);

                    if (opponent != null)
                    {
                        var scores = matchData["Result"].Split('-');
                        int teamScore = int.Parse(scores[0]);
                        int opponentScore = int.Parse(scores[1]);

                        var h2hMatch = new H2H(opponent, teamScore, opponentScore)
                        {
                            IsExibitionMatch = true
                        };
                        team.H2HMatches.Add(h2hMatch);
                    }
                }
            }
        }

    }
}
