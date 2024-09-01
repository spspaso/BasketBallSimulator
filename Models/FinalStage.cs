using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class FinalStage
    {
        public Match FinalMatch { get; private set; }
        public Match ThirdPlaceMatch { get; private set; }

        public FinalStage(Match finalMatch, Match thirdPlaceMatch)
        {
            FinalMatch = finalMatch;
            ThirdPlaceMatch = thirdPlaceMatch;
        }
    }
}
