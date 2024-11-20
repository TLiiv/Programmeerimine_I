using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IUsersService
    {
        Task<List<User>> AllUsers();
        Task<User> Get(int id);
        Task Save(User user);
        Task Delete(int id);
        
    }
}
