using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class Leaderboard
    {
        [Key]
        public Guid LeaderBoardId { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public ICollection<User>? LeaderboardUsers { get; set; }

        [ForeignKey(nameof(Tournament))]
        public Guid TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public int PredictedPoints { get; set; } = 0;
        public int Rank { get; set; } = 0;
    }
}