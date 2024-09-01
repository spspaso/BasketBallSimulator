using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class QuarterFinalStage
    {
        public List<Match> Matches { get; private set; }

        public QuarterFinalStage(List<Match> matches)
        {
            if (matches.Count != 4)
                throw new ArgumentException("QuarterFinalStage must have exactly 4 matches.");

            Matches = matches;
        }
    }
}
