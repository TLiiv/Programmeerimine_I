using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using KooliProjekt.Services;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class UserControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public UserControllerTests() 
        {
            _client = Factory.CreateClient();
            //Get fake db data
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
            
        }
        //GET METHODS
        [Theory]
        [InlineData("/Users")]
        [InlineData("/Users/Create")]
        public async Task Get_endpoints_return_success_and_correct_content_type(string url)
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
       
        [Theory]
        [InlineData("/Users/Details/")]
        [InlineData("/Users/Edit/")]
        [InlineData("/Users/Delete/")]
        public async Task Get_endpoints_should_return_not_found_when_user_id_is_missing(string url)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/Users/Details/1")]
        [InlineData("/Users/Edit/1")]
        [InlineData("/Users/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_user_does_not_exist(string url)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
           
        }
       
        [Fact]
        public async Task Details_should_return_succsess_when_user_is_found()
        {
            //Arrange
            var user = new User { UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com",Password="asd", PhoneNumber = "51231231", IsAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();
            //Act
            using var response = await _client.GetAsync("/Users/Details/" + user.UserId);
            //Assert
            response.EnsureSuccessStatusCode();
            //Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
        
      
        [Fact]
        public async Task Edit_should_return_succsess_when_existing_user_is_found()
        {
            //Arrange
            var user = new User { UserId = Guid.NewGuid(), UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com", Password = "asd", PhoneNumber = "51231231", IsAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();
            //Act
            using var response = await _client.GetAsync("/Users/Edit/" + user.UserId);
            //Assert
            response.EnsureSuccessStatusCode();
        }
       
        [Fact]
        public async Task Delete_should_return_succsess_when_existing_user_is_found()
        {
            //Arrange
            var user = new User { UserId = Guid.NewGuid(), UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com", Password = "asd", PhoneNumber = "51231231", IsAdmin = true };
            _context.Users.Add(user);
            _context.SaveChanges();
            //Act
            using var response = await _client.GetAsync("/Users/Delete/" + user.UserId);
            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
//Refactored code
//[Fact]
//public async Task Index_should_return_success()
//{
//    //Arrange
//    //using var client = Factory.CreateClient();
//    //Act
//    using var response = await _client.GetAsync("/Users");
//    //Assert
//    response.EnsureSuccessStatusCode();
//    //Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
//}
//[Fact]
//public async Task Create_should_return_success()
//{
//    //Arrange
//    //using var client = Factory.CreateClient();
//    //Act
//    using var response = await _client.GetAsync("/Users/Create");
//    //Assert
//    response.EnsureSuccessStatusCode();
//}
//[Fact]
//public async Task Details_should_return_not_found_when_id_is_missing()
//{
//    //Arrange

//    //Act
//    using var response = await _client.GetAsync("/Users/Details/");

//    //Assert
//    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
//}
//[Fact]
//public async Task Details_should_return_not_found_when_user_is_missing()
//{
//    //Arrange

//    //Act
//    using var response = await _client.GetAsync("/Users/Details/1");

//    //Assert
//    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
//}

//[Fact]
//public async Task Edit_should_return_not_found_when_id_is_missing()
//{
//    //Arrange

//    //Act
//    using var response = await _client.GetAsync("/Users/Edit/");

//    //Assert
//    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
//}
//[Fact]
//public async Task Edit_should_return_not_found_when_user_is_missing()
//{
//    //Arrange

//    //Act
//    using var response = await _client.GetAsync("/Users/Edit/1");

//    //Assert
//    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
//}
//[Fact]
//public async Task Delete_should_return_not_found_when_id_is_missing()
//{
//    //Arrange

//    //Act
//    using var response = await _client.GetAsync("/Users/Delete/");

//    //Assert
//    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
//}
//[Fact]
//public async Task Delete_should_return_not_found_when_user_is_missing()
//{
//    //Arrange

//    //Act
//    using var response = await _client.GetAsync("/Users/Delete/1");

//    //Assert
//    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
//}