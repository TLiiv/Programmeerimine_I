using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class LeaderboardsService : ILeaderboardsService
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Leaderboard>> AllLeaderboards()
        {
            return await _context.Leaderboards
                .Include(l => l.User)
                .Include(l => l.Tournament)
                .ToListAsync();
        }

        public async Task<Leaderboard> Get(Guid id)
        {
            return await _context.Leaderboards
                .Include(l => l.Tournament)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LeaderBoardId == id);
        }

        public async Task<(List<Tournament> Tournaments, List<User> Users)> GetDropdownData()
        {
           
            var Tournaments = await _context.Tournaments.ToListAsync();
            var Users = await _context.Users.ToListAsync();

            return (Tournaments, Users);
        }

        public async Task Save(Leaderboard leaderboard)
        {
            if (leaderboard.LeaderBoardId == Guid.Empty) 
            {
                leaderboard.LeaderBoardId = Guid.NewGuid();
                _context.Add(leaderboard);
            }
            else
            {
                _context.Update(leaderboard);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await _context.Leaderboards
                .Where(leaderboard=>leaderboard.LeaderBoardId == id)
                .ExecuteDeleteAsync();
        }
    }
}
