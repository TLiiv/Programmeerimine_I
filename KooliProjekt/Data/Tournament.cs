using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Tournament
    {
        [Key]
        public Guid TournamentId { get; set; } = Guid.NewGuid();
        
        [Required(ErrorMessage = "Tournament name is required.")]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Tournament name must be between 5 and 40 characters.")]
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

        
        public TournamentStatus Status { get; set; }

    }

    public enum TournamentStatus
    {
        [Display(Name = "Not started")]
        NotStarted,

        [Display(Name = "Ongoing")]
        Ongoing,

        [Display(Name = "Finished")]
        Finished
    }
}
