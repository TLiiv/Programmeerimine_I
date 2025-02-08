using Azure.Identity;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{

    public class UserControllerTests
    {
        private readonly Mock<IUsersService> _userServiceMock;
        private readonly UsersController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUsersService>();
            _controller = new UsersController(_userServiceMock.Object);
        }

        //GET
        [Fact]
        public async Task Index_should_return_correct_view_and_data()
        {
            // Arrange
            //var userServiceMock = new Mock<IUsersService>();
            //var controller = new UsersController(userServiceMock.Object); //Made it global

            var data = new List<User>
        {
            new User { UserId = Guid.NewGuid(), UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com",PhoneNumber="51231231", IsAdmin = true },
            new User { UserId = Guid.NewGuid(), UserName = "user2", FirstName = "Second", LastName = "User", Email = "user2@example.com",PhoneNumber="51231231", IsAdmin = false }
         };

            _userServiceMock.Setup(service => service.AllUsers()).ReturnsAsync(data);
            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert

            //viewname check
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));
            //view data check
            var model = result.Model as List<User>;
            Assert.NotNull(model); // Ensure the model is not null
            Assert.Equal(2, model.Count); // Ensure the correct number of users are returned
            Assert.Equal("user1", model[0].UserName); // Check first user
            Assert.Equal("user2", model[1].UserName); // Check second user
        }


        [Fact]
        public async Task Details_should_return_correct_view_and_data()
        {
            // Arrange
            //var userServiceMock = new Mock<IUsersService>();
            //var controller = new UsersController(userServiceMock.Object); //Made it global


            var id = Guid.NewGuid();//create userId variable

            var data = new List<User>
        {
            new User { UserId = id, UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com",PhoneNumber="51231231", IsAdmin = true },
         };

            _userServiceMock.Setup(service => service.Get(id)) // `userId` is passed to mock `Get`
                           .ReturnsAsync(data.FirstOrDefault(u => u.UserId == id)); // Look for the `UserId` in the list that matches the `userId` variable

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert

            //viewname check
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Details" ||
                        string.IsNullOrEmpty(result.ViewName));
            //view data check
            var model = result.Model as User;  // Get the model from the view
            Assert.NotNull(model);  // Ensure the model is not null
            Assert.Equal(id, model.UserId);  // Check if the correct userId is returned
            Assert.Equal("user1", model.UserName);  // Check if the correct userName is returned
        }

        //POST
        //Create
        [Fact]
        public async Task Create_Should_Redirect_After_Succeful_User_Creation()
        {
            //Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "testUser",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "password",
                PhoneNumber = "123456789",
                IsAdmin = false
            };
            _userServiceMock
                .Setup(service => service.Save(user))
                .Returns(Task.CompletedTask)
                .Verifiable();
            //Act
            var result = await _controller.Create(user) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _userServiceMock.VerifyAll(); //check that save method is used
        }
        [Fact]
        public async Task Create_Should_Stay_On_View_When_Model_Is_Not_Valid()
        {
            //Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "testUser",
                FirstName = "Test",
                LastName = "User",
                Email = "",
                Password = "password",
                PhoneNumber = "123456789",
                IsAdmin = false
            };

            _userServiceMock
               .Setup(service => service.Save(user))
               .Returns(Task.CompletedTask);

            //Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(user) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Create" ||
                        string.IsNullOrEmpty(result.ViewName));
        }


        //Edit
        [Fact]
        public async Task Edit_Should_Return_Not_Found_When_UserId_Is_Missing()
        {

            // Arrange
            var invalidUserId = Guid.NewGuid();
            var user = new User
            {
                UserId = Guid.NewGuid(),  // Different ID than invalidUserId
                UserName = "testUser",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "password",
                PhoneNumber = "123456789",
                IsAdmin = false
            };


            // Act
            var result = await _controller.Edit(invalidUserId, user);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
        }



        [Fact]
        public async Task Edit_Should_Return_View_With_Changed_User_Info()
        {
            //Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "testUser",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "password",
                PhoneNumber = "123456789",
                IsAdmin = false
            };
            _userServiceMock
                .Setup(service => service.Save(user))
                .Returns(Task.CompletedTask)
                .Verifiable();
            //Act
            var result = await _controller.Edit(user.UserId, user) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Edit" ||
                        string.IsNullOrEmpty(result.ViewName));
            _userServiceMock.VerifyAll(); //check that save method is used
        }

        [Fact]
        public async Task Edit_Should_Return_Error_When_Model_Not_Valid()
        {
            //Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "testUser",
                FirstName = "Test",
                LastName = "User",
                Email = "",
                Password = "password",
                PhoneNumber = "123456789",
                IsAdmin = false
            };

            _userServiceMock
                .Setup(service => service.Save(user))
                .Returns(Task.CompletedTask);


            //Act
            _controller.ModelState.AddModelError("error", "error"); //Give error to model and
            var result = await _controller.Edit(user.UserId, user) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.False(result.ViewData.ModelState.IsValid); //check if the error works and modelstate is false

        }
        [Fact]
        public async Task DeleteConfirmed_Should_Redirect_On_Correct_Id_And_Successful_Delete()
        {
            //Arrange
            var userId = Guid.NewGuid();
            _userServiceMock
                .Setup(service => service.Delete(userId))
                .Returns(Task.CompletedTask)
                .Verifiable();
            //Act

            var result = await _controller.DeleteConfirmed(userId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _userServiceMock.VerifyAll();
        }

    }

}
