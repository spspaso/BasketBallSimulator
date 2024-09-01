using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class SemiFinalStage
    {
        public List<Match> Matches { get; private set; }

        public SemiFinalStage(List<Match> matches)
        {
            if (matches.Count != 2)
                throw new ArgumentException("SemiFinalStage must have exactly 2 matches.");

            Matches = matches;
        }
    }
}
