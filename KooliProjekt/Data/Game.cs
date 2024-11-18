using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Game
    {
        [Key]
        public Guid GamesId { get; set; } = Guid.NewGuid();

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime GameStartDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime GameStartTime { get; set; }

        
        [ForeignKey(nameof(HomeTeamId))]
        public Guid? HomeTeamId { get; set; } 
        public Team? HomeTeam { get; set; }

        
        [ForeignKey(nameof(AwayTeamId))]
        public Guid? AwayTeamId { get; set; } 
        public Team? AwayTeam { get; set; }

        public IList<UserBets> UserBets { get; set; } = new List<UserBets>();
        public bool AreTeamsConfirmed { get; set; }

        [Required]
        [ForeignKey(nameof(Tournament))]
        public Guid TournamentId { get; set; }
        public Tournament? Tournament { get; set; }
    }
}