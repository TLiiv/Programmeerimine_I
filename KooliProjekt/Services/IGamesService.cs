using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IGamesService
    {
        Task<List<Game>> AllGames(GamesSearch search = null);
        Task<Game> Get(Guid id);

        Task Save(Game game);
        Task Delete(Guid id);
        Task<(List<Team> Teams, List<Tournament> Tournaments)> GetDropdownData();
    }
}
