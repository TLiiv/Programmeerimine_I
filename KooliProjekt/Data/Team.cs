using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class Team
    {
        public Guid TeamId { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(100)]
        public string TeamName { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsScoredAgainstThem {  get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public int GamesPlayed { get; set; }
        [ForeignKey("Tournament")]
        public Guid TornamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
