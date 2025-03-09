namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<List<User>> AllUsers();
        Task Save(User user);
        Task Delete(Guid id);
    }
}