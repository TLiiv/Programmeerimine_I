using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp.Api
{
    public class UserBets
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TournamentId { get; set; }
        public Guid GameId { get; set; }
        public Guid PredictedWinningTeamId { get; set; }
        public int PredictedHomeGoals { get; set; }
        public int PredictedAwayGoals { get; set; }
        public double AccountBalance { get; set; }
        public double BetAmount { get; set; }
        public DateTime BetPlacedDate { get; set; }
    }
}
