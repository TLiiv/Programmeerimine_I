using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _context;

        public GamesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> AllGames() 
        { 
            return await _context.Games
                .Include(g => g.AwayTeam)
                .Include(g => g.HomeTeam)
                .Include(g => g.Tournament)
                .ToListAsync();
        }

        public async Task<Game>Get(Guid id)
        {
            return await _context.Games
                .Include(g => g.AwayTeam)
                .Include(g => g.HomeTeam)
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(m => m.GamesId == id);
        }

        public async Task<(List<Team> Teams, List<Tournament> Tournaments)> GetDropdownData()
        {
            var teams = await _context.Teams.ToListAsync();
            var tournaments = await _context.Tournaments.ToListAsync();

            return (teams, tournaments);
        }

        public async Task Save(Game game)
        {
            if(game.GamesId == Guid.Empty)
            {
                game.GamesId = Guid.NewGuid();
                _context.Add(game);
            }
            else
            {
                _context.Update(game);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await _context.Games
                .Where(game => game.GamesId == id)
                .ExecuteDeleteAsync();
        }
    }
}
