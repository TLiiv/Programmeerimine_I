using System.Net.Http;
using System.Net.Http.Json;

namespace KooliProjekt.WpfApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7136/");
        }

        public async Task<List<User>> AllUsers() 
        {
            var result = await _httpClient.GetFromJsonAsync<List<User>>("Users/api/allusers");
            return result ?? new List<User>();
        }



        public async Task Save(User user)
        {
            if (user.UserId == Guid.Empty) 
            {
                // Create a new user
                await _httpClient.PostAsJsonAsync("Users/api/save", user);
            }
            else
            {
                // Update existing user
                await _httpClient.PutAsJsonAsync("Users/api/save", user); 
            }
        }


        public async Task Delete(Guid userId)
        {
          
            var response = await _httpClient.DeleteAsync($"Users/api/delete/{userId}");

        }
    }
}