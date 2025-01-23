using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class UserBetsControllerTests
    {
        [Fact]
        public async void Index_should_return_correct_view_and_data()
        {
            //data needed User 	Tournament Game,PredictedWinningTeam,PredictedHomeGoals,PredictedAwayGoals ,AccountBalance BetAmount BetPlacedDate 	

            // Arrange
            var userServiceMock = new Mock<IUserBetsService>();
            var controller = new UserBetsController(userServiceMock.Object);

            var data = new List<UserBets>
            {
                
            };

            userServiceMock.Setup(service => service.AllUserBets()).ReturnsAsync(data);
            // Act
            var result = await controller.Index() as ViewResult;

            // Assert

            //viewname check
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));
            //view data check
        //    var model = result.Model as List<User>;
        //    Assert.NotNull(model); // Ensure the model is not null
        //    Assert.Equal(2, model.Count); // Ensure the correct number of users are returned
        //    Assert.Equal("user1", model[0].UserName); // Check first user
        //    Assert.Equal("user2", model[1].UserName); // Check second user
        }

    }
}
