using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ITeamsService
    {
        Task<List<Team>> AllTeams(TeamsSearch search = null);

        Task<Team> Get(Guid id);
        Task Save(Team team);
        Task Delete(Guid id);
    }
}
