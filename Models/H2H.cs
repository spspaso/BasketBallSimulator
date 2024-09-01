using CodeBehind.BasketBallSimulator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class H2H
    {
        public Team Opponent { get; set; }

        public int Score { get; set; }

        public int OpponentScore { get; set; }

        public bool IsExibitionMatch { get; set; }

        public MatchOutcome MatchOutcome => Score > OpponentScore ? MatchOutcome.WIN : MatchOutcome.LOSE;

        public H2H(Team opponent, int score, int opponentScore)
        {
            Opponent = opponent;
            Score = score;
            OpponentScore = opponentScore;
        }
    }
}
