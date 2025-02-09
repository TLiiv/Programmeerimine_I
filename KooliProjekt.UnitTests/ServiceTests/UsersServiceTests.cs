using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class UsersServiceTests : ServiceTestBase
    {
        private readonly UsersService _service;
        public UsersServiceTests() 
        { 
            _service = new UsersService(DbContext);
        }
        [Fact]
        public async Task AllUsers_Should_Get_All_Users()
        {
            //Fake data for blackbox testing to send into InMemory DB (ServiceTestBase)

            // Arrange

            var user1 = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "TestUser1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!"
            };
            var user2 = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "TestUser2",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Password = "SecurePass456!"
            };
          
            await DbContext.AddAsync(user1);
            await DbContext.AddAsync(user2);
            await DbContext.SaveChangesAsync();

            //Act
            await _service.AllUsers();
            
            //Assert
            var count = DbContext.Users.Count();
            Assert.Equal(2, count);
        }
        [Fact]
        public async Task Get_Should_Fetch_User_By_Id()
        {
            // Arrange

            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "TestUser1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                UserBets = new List<UserBets>
                {
                     new UserBets { Id = Guid.NewGuid(), BetAmount = 50.0, AccountBalance = 500.0 },
                     new UserBets { Id = Guid.NewGuid(), BetAmount = 25.0, AccountBalance = 400.0 }
                }
            };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            //Act
            var result = await _service.Get(user.UserId);

            // Assert
            Assert.NotNull(result); // Ensure the user is fetched
            Assert.Equal(user.UserId, result.UserId);
            Assert.NotNull(result.UserBets);
            Assert.Equal(2, result.UserBets.Count);
        }

        [Fact]
        public async Task Save_Should_Save_User_And_Add_Id() 
        {
            //Arrange
            var user = new User
            {
                UserName = "TestUser1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
            };
            
            //Act
            await _service.Save(user);

            //Assert
            var savedUser = await DbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            Assert.NotNull(savedUser);
            Assert.NotEqual(Guid.Empty, savedUser.UserId);
        }

        [Fact]
        public async Task Save_Should_Update_Existing_User_When_UserId_Is_Provided()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = Guid.NewGuid(), 
                UserName = "OldUser",
                FirstName = "John",
                LastName = "Doe",
                Email = "old.doe@example.com",
                Password = "OldPassword123!"
            };

            // Add the user to the database first
            await DbContext.AddAsync(existingUser);
            await DbContext.SaveChangesAsync();

            // Modify the user
            existingUser.UserName = "UpdatedUser";

            // Act
            await _service.Save(existingUser);

            // Assert
            var updatedUser = await DbContext.Users.FirstOrDefaultAsync(u => u.UserId == existingUser.UserId);
            Assert.NotNull(updatedUser);
            Assert.Equal(existingUser.UserName, updatedUser.UserName);
        }

            //[Fact]
            //public async Task Delete_Should_Remove_User() //ExecuteDeleteAsync() Cannot be used with InMemory deps..
            //{
            //    //Arrange
            //    //Fake data for blackbox testing to send into InMemory DB (ServiceTestBase)

            //    // Arrange
            //    var user = new User
            //    {
            //        Email = "test.user@example.com",
            //        FirstName = "Test",
            //        LastName = "User",
            //        Password = "SecurePassword123!",
            //        UserName = "testuser"
            //    };

            //    await DbContext.AddAsync(user);
            //    await DbContext.SaveChangesAsync();

            //    // Act
            //    await _service.Delete(user.UserId);

            //    // Assert
            //    var count  = DbContext.Users.Count();
            //    Assert.Equal(0, count);
            //}

        }

}
