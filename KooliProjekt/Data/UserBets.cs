using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class UserBets
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Tournament")]
        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        [ForeignKey("Game")]
        public Guid GameId { get; set; }
        public Game Game { get; set; }
        
        [ForeignKey("Team")]
        public Guid PredictedWinningTeamId { get; set; }
        public Team PredictedWinningTeam { get; set; }

        public int PredictedHomeGoals { get; set; }
        public int PredictedAwayGoals { get; set; }
        public int Score { get; set; }
        public double AccountBalance { get; set; }
        public double BetAmount { get; set; }

        
    }
}
