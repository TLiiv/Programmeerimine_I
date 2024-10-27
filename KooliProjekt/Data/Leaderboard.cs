using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class Leaderboard
    {
        [Key]
        public Guid LeaderBoardId { get; set; } = Guid.NewGuid();
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public ICollection<User> Users { get; set; }
        [ForeignKey("Tournament")]
        public Guid TournamentId {  get; set; }
        public Tournament Tournament { get; set; }
        public int PredictedPoints { get; set; } = 0;
        public int Rank { get; set; } = 0;
    }
}
