using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface ILeaderboardsService
    {
        Task<List<Leaderboard>> AllLeaderboards();

        Task<Leaderboard> Get(Guid id);

        Task<(List<Tournament> Tournaments, List<User> Users)> GetDropdownData();
        Task Save(Leaderboard leaderboard);
        Task Delete(Guid id);
    }
}
