using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class Game
    {
        public Guid GamesId { get; set; } = Guid.NewGuid();

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime GameStartDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime GameStartTime { get; set; }

        [Required]
        [ForeignKey("Team")]
        public Guid? HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }

        [ForeignKey("Team")]
        public Guid? AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }
       
        public ICollection<UserBets> UserBets { get; set; } = new List<UserBets>();
        public bool AreTeamsConfirmed { get; set; }

        [Required]
        [ForeignKey("Tournament")]
        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
