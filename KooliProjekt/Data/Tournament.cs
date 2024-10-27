using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Tournament
    {
        [Key]
        public Guid TournamentId { get; set; } = Guid.NewGuid();
        [Required]
        public string TournamentName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TournamentStartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TournamentEndtDate { get; set; }
        public ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();
        public ICollection<Game> Games { get; set; } = new List<Game>();
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public string Status { get; set; }

    }
}
