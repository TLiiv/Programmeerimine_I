using Azure;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            // Turn of Redirection trackin (for post mehtod status codes (after user save etc there is redirection))
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false}; 
            _client = Factory.CreateClient(options);
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
        [InlineData("/Users/Details/1")]
        [InlineData("/Users/Edit/1")]
        [InlineData("/Users/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_user_id_is_missing_or_does_not_exist(string url)
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

        //POST
        [Fact]
        public async Task Create_should_save_new_user() 
        {
            //Arrange
            //Add form data
            var formValues =new Dictionary<string, string>();
            
            formValues.Add("UserName", "user1");
            formValues.Add("FirstName", "First");
            formValues.Add("LastName", "User");
            formValues.Add("Email","user1@example.com");
            formValues.Add("Password", "asd");
            formValues.Add("PhoneNumber", "51231231");
            formValues.Add("IsAdmin","true");

            using var content = new FormUrlEncodedContent(formValues);
            
            //Act
            using var response = await _client.PostAsync("/Users/Create",content);

            //Assert
            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently);

            //check if it created a user
            var user = _context.Users.FirstOrDefault();
            Assert.NotNull(user);
            Assert.NotEqual(Guid.Empty, user.UserId);
            Assert.Equal("user1",user.UserName);
        }

        [Fact]
        public async Task Create_should_not_save_new_user_with_invalid_data()
        {
            //Arrange
            var formValues = new Dictionary<string, string>();

            formValues.Add("UserName", "");         

            using var content = new FormUrlEncodedContent(formValues);

            //Act
            using var response = await _client.PostAsync("/Users/Create", content);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.False(_context.Users.Any());
        }

        [Fact]
        public async Task Edit_should_update_user()
        {
            //Arrange
            //Add form data
            var formValues = new Dictionary<string, string>();

            formValues.Add("UserName", "user1");
            formValues.Add("FirstName", "First");
            formValues.Add("LastName", "User");
            formValues.Add("Email", "user1@example.com");
            formValues.Add("Password", "asd");
            formValues.Add("PhoneNumber", "51231231");
            formValues.Add("IsAdmin", "true");
           

            using var content = new FormUrlEncodedContent(formValues);
           

            //Act
            using var response = await _client.PostAsync("/Users/Create", content);

            
            // Retrieve user from DB
            var user = _context.Users.FirstOrDefault();

            Assert.NotNull(user);
            //Else it still tracks the created user instead of edited user
            _context.Entry(user).State = EntityState.Detached;

            var editFormValues = new Dictionary<string, string>
            {
                { "UserId", user.UserId.ToString() },
                { "UserName", "updatedUser" },
                { "FirstName", "UpdatedFirst" },
                { "LastName", "UpdatedLast" },
                { "Email", "updated@example.com" },
                { "Password", "newpass" },
                { "PhoneNumber", "99999999" },
                { "IsAdmin", "false" }
            };



            using var editContent = new FormUrlEncodedContent(editFormValues);
           
            using var editResponse = await _client.PostAsync($"/Users/Edit/{user.UserId}", editContent);


            //Assert

            //200 no redirection in edit
            editResponse.EnsureSuccessStatusCode();

            //check if it updated a user
            var updatedUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            Assert.NotNull(updatedUser);
            Assert.Equal("updatedUser", updatedUser.UserName);
        }

        [Fact]
        public async Task Edit_should_not_update_user_when_incorrect_data_added()
        {
            //Arrange
            //Add form data
            var formValues = new Dictionary<string, string>();

            formValues.Add("UserName", "user1");
            formValues.Add("FirstName", "First");
            formValues.Add("LastName", "User");
            formValues.Add("Email", "user1@example.com");
            formValues.Add("Password", "asd");
            formValues.Add("PhoneNumber", "51231231");
            formValues.Add("IsAdmin", "true");

            using var content = new FormUrlEncodedContent(formValues);


            //Act
            using var response = await _client.PostAsync("/Users/Create", content);



            // Retrieve user from DB
            var user = _context.Users.FirstOrDefault();

            Assert.NotNull(user);
            //Else it still tracks the created user instead of edited user
            _context.Entry(user).State = EntityState.Detached;

            // Modify user data for edit
            var editFormValues = new Dictionary<string, string>
            {
                { "UserId", user.UserId.ToString() },
                { "UserName", "" },
                { "FirstName", "UpdatedFirst" },
                { "LastName", "UpdatedLast" },
                { "Email", "updated@example.com" },
                { "Password", "newpass" },
                { "PhoneNumber", "99999999" },
                { "IsAdmin", "false" }
            };



            using var editContent = new FormUrlEncodedContent(editFormValues);

            using var editResponse = await _client.PostAsync($"/Users/Edit/{user.UserId}", editContent);


            //Assert

            //200 no redirection in edit
            editResponse.EnsureSuccessStatusCode();

            //check if it updated a user
            var updatedUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            Assert.NotNull(updatedUser);
            Assert.Equal("user1", updatedUser.UserName);
        }

        [Fact]
        public async Task Delete_should_delete_user()
        {
            //Arrange
            //Add form data
            var formValues = new Dictionary<string, string>();

            formValues.Add("UserName", "user1");
            formValues.Add("FirstName", "First");
            formValues.Add("LastName", "User");
            formValues.Add("Email", "user1@example.com");
            formValues.Add("Password", "asd");
            formValues.Add("PhoneNumber", "51231231");
            formValues.Add("IsAdmin", "true");


            using var content = new FormUrlEncodedContent(formValues);


            //Act
            using var response = await _client.PostAsync("/Users/Create", content);


            // Retrieve user from DB
            var user = _context.Users.FirstOrDefault();

            Assert.NotNull(user);
            //Else it still tracks the created user instead of edited user
            _context.Entry(user).State = EntityState.Detached;




            using var deleteResponse = await _client.PostAsync($"/Users/Delete/{user.UserId}", null); 

            // Assert - Check for redirect after successful deletion 
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.Redirect || deleteResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Assert - Ensure the user was deleted from DB
            var deletedUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            Assert.Null(deletedUser);  // Ensure the user no longer exists in the database
        }
    }
}
