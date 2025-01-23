﻿using KooliProjekt.Controllers;
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
        [Fact]
        public async void Index_should_return_correct_view_and_data()
        {
            // Arrange
            var userServiceMock = new Mock<IUsersService>();
            var controller = new UsersController(userServiceMock.Object);

            var data = new List<User>
        {
            new User { UserId = Guid.NewGuid(), UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com",PhoneNumber="51231231", IsAdmin = true },
            new User { UserId = Guid.NewGuid(), UserName = "user2", FirstName = "Second", LastName = "User", Email = "user2@example.com",PhoneNumber="51231231", IsAdmin = false }
         };

            userServiceMock.Setup(service => service.AllUsers()).ReturnsAsync(data);
            // Act
            var result = await controller.Index() as ViewResult;

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
        public async void Details_should_return_correct_view_and_data()
        {

        }
    }
    
}
