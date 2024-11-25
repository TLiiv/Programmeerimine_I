using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace KooliProjekt.Services
{
    public class TeamsService:ITeamsService
    {

        private readonly ApplicationDbContext _context;

        public TeamsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> AllTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> Get(Guid id)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(m => m.TeamId == id);
        }

        public async Task Save(Team team) 
        { 
            if(team.TeamId == Guid.Empty) 
            { 
                team.TeamId = Guid.NewGuid();
                _context.Teams.Add(team);
            }
            else
            {
                _context.Teams.Update(team);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await _context.Teams
               .Where(team => team.TeamId == id)
               .ExecuteDeleteAsync();
        }
    }
}
