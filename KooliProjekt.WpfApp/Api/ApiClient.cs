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

        public async Task<Result<List<User>>> AllUsers() 
        {
            //var result = await _httpClient.GetFromJsonAsync<List<User>>("Users/api/allusers");
            var result = new Result<List<User>>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<User>>("Users/api/allusers");
            }
            catch (HttpRequestException ex)
            {
                if (ex.HttpRequestError == HttpRequestError.ConnectionError)
                {
                    result.Error = "Ei saa serveriga ühendust. Palun proovi hiljem uuesti.";
                }
                else { result.Error = ex.Message; }

            }
            catch (Exception ex) 
            {
                result.Error = ex.Message;
            }

            return result;
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
                await _httpClient.PutAsJsonAsync($"Users/api/update/{user.UserId}", user); 
                //await _httpClient.PostAsJsonAsync($"Users/api/save/{user.UserId}", user); 

            }
        }

      
        public async Task Delete(Guid userId)
        {
          
            var response = await _httpClient.DeleteAsync($"Users/api/delete/{userId}");

        }
    }
}

//public async Task Save(User user)
//{
//    if (user.UserId == Guid.Empty)
//    {
//        await _httpClient.PostAsJsonAsync("Users/api/create", user);
//    }
//    else
//    {
//        await _httpClient.PutAsJsonAsync("TodoLists/" + list.Id, list);
//    }
//}