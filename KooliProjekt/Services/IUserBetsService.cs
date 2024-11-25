using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IUserBetsService
    {
        Task<List<UserBets>> AllUserBets();
        Task<UserBets> Get(Guid id);
        Task<(List<Game> Games, List<Team> Teams, List<Tournament> Tournaments, List<User> Users)> GetDropdownData();

        Task Save(UserBets userBets);
        Task Delete(Guid id);
    }
}
