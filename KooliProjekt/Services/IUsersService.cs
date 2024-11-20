using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IUsersService
    {
        Task<List<User>> AllUsers();
        Task<User> Get(Guid id);
        Task Save(User user);
        Task Delete(Guid id);
        
    }
}
