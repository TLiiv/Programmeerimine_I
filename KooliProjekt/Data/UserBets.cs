using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class UserBets
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [ForeignKey(nameof(Tournament))]
        public Guid TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        [Required]
        [ForeignKey(nameof(Game))]
        public Guid GameId { get; set; }
        public Game? Game { get; set; }

        [Required]
        [ForeignKey(nameof(PredictedWinningTeam))]
        public Guid PredictedWinningTeamId { get; set; }
        public Team? PredictedWinningTeam { get; set; }

        [Range(0, int.MaxValue)]
        public int PredictedHomeGoals { get; set; }

        [Range(0, int.MaxValue)]
        public int PredictedAwayGoals { get; set; }

        [Range(0, double.MaxValue)]
        public double AccountBalance { get; set; }

        [Range(0, double.MaxValue)]
        public double BetAmount { get; set; }
        public DateTime BetPlacedDate { get; set; } = DateTime.UtcNow;

    }
}