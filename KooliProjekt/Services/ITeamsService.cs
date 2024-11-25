using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface ITeamsService
    {
        Task<List<Team>> AllTeams();

        Task<Team> Get(Guid id);
        Task Save(Team team);
        Task Delete(Guid id);
    }
}
