using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface ITournamentsService
    {
        Task<List<Tournament>> AllTournaments();
        
        Task<Tournament> Get(Guid id);
        Task Save(Tournament tournament);
        Task Delete(Guid id);
    }
}
