using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class Team
    {
        public Guid TeamId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Team name is required.")]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Team name must be between 5 and 40 characters.")]
        public string TeamName { get; set; }
        
        public int GoalsScored { get; set; } = 0;
        public int GoalsScoredAgainstThem { get; set; } = 0;
        public int GamesWon { get; set; } = 0;
        public int GamesLost { get; set; } = 0;
        public int GamesPlayed { get; set; } = 0;
        
        [ForeignKey("Tournament")]
        public Guid TornamentId { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
