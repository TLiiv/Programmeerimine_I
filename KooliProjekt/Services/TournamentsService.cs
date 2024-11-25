using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class TournamentsService : ITournamentsService
    {
        private readonly ApplicationDbContext _context;

        public TournamentsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Tournament>> AllTournaments() 
        { 
            return await _context.Tournaments.ToListAsync();
        }

        

        public async Task<Tournament> Get(Guid id) 
        {
            return await _context.Tournaments
                .FirstOrDefaultAsync(m => m.TournamentId == id);
        }

        public async Task Save(Tournament tournament) 
        {
            if (tournament.TournamentId == Guid.Empty) 
            {
                tournament.TournamentId = Guid.NewGuid();
                _context.Add(tournament);
            }
            else
            {
                _context.Update(tournament);
            }
            await _context.SaveChangesAsync();

        }

        public async Task Delete(Guid id)
        {
           await _context.Tournaments
                .Where(m => m.TournamentId == id)
                .ExecuteDeleteAsync();
        }
      
    }
}
