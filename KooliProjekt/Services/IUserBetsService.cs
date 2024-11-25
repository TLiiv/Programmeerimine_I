using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IUserBetsService
    {
        Task<List<UserBets>> AllUserBets();
        Task<UserBets> Get(Guid id);
        Task Save(UserBets userBets);
        Task Delete(Guid id);
    }
}
