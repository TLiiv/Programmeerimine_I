using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class UserBetsService : IUserBetsService
    {
        private readonly ApplicationDbContext _context;

        public UserBetsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserBets>> AllUserBets() 
        {
            return await _context.UsersBets
                .Include(u => u.User)
                .Include(u => u.Game)
                .Include(u => u.PredictedWinningTeam)
                .Include(u => u.Tournament)
                .ToListAsync(); 
        }

        public async Task<UserBets> Get(Guid id) 
        {
            return await _context.UsersBets
                .Include(u => u.Game)
                .Include(u => u.PredictedWinningTeam)
                .Include(u => u.Tournament)
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<(List<Game> Games, List<Team> Teams, List<Tournament> Tournaments, List<User> Users)> GetDropdownData()
        {
            var Games = await _context.Games.ToListAsync();
            var Teams = await _context.Teams.ToListAsync();
            var Tournaments = await _context.Tournaments.ToListAsync();
            var Users = await _context.Users.ToListAsync();

            return (Games, Teams, Tournaments, Users);
        }

        public async Task Save(UserBets userBets)
        {
            if (userBets.Id == Guid.Empty)
            {
                userBets.BetPlacedDate = DateTime.UtcNow;
                userBets.Id = Guid.NewGuid();
                _context.UsersBets.Add(userBets);
            }
            else
            {
                _context.UsersBets.Update(userBets);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
               await _context.UsersBets
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
