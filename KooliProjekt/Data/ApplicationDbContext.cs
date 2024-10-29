using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBets> UsersBets { get; set; }
    }
}