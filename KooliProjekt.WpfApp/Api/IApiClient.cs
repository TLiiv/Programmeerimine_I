namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        //Task<List<TodoList>> List();
        //Task Save(TodoList list);
        //Task Delete(int id);
        Task<List<User>> AllUsers();
        Task Save(User user);
        Task Delete(Guid id);
    }
}