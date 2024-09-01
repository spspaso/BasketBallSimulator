using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind.BasketBallSimulator.Models
{
    public class Tournament
    {
        public GroupStage GroupStage { get; set; }

        public KnockOutStage KnockOutStage { get; set; }

        public Tournament()
        {
            GroupStage = new GroupStage();
        }

        private void ExecuteGroupStage()
        {
            GroupStage.SimulateGroupStage();
            GroupStage.DisplayRoundMatches();
            GroupStage.DisplayGroupsStandings();
        }

        private void ExecuteKnockOutStage()
        {
            KnockOutStage.SimulateKnockoutStage();
            KnockOutStage.DisplayKnockoutResults();
        }

        public void ExecuteTournament()
        {
            ExecuteGroupStage();

            var teamsForKnockOutPhaseDraw = GroupStage.GetTeamsForKnockOutPhaseDraw();
            KnockOutStage = new KnockOutStage(teamsForKnockOutPhaseDraw);

            ExecuteKnockOutStage();
        }
    }
}
