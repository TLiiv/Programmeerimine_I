using Azure.Identity;
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
            // Arrange
            var userBetsServiceMock = new Mock<IUserBetsService>();
            var controller = new UserBetsController(userBetsServiceMock.Object);

            var data = new List<UserBets>
            {
                new UserBets
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    User = new User{UserId = Guid.NewGuid(), UserName = "Test User" },
                    GameId = Guid.NewGuid(),
                    Game = new Game{GamesId = Guid.NewGuid()},
                    PredictedWinningTeamId = Guid.NewGuid(),
                    PredictedWinningTeam = new Team { TeamId = Guid.NewGuid()},
                    TournamentId = Guid.NewGuid(),
                    Tournament = new Tournament { TournamentId = Guid.NewGuid()},
                    PredictedHomeGoals = 1,
                    PredictedAwayGoals = 2,
                    AccountBalance = 100.0,
                    BetAmount = 10.0,
                    BetPlacedDate = DateTime.UtcNow
                },
            };

            userBetsServiceMock.Setup(service => service.AllUserBets()).ReturnsAsync(data);
            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            //viewname check
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));
            //view data check
            var model = result.Model as List<UserBets>;
            Assert.NotNull(model);
            Assert.Equal(data.Count, model.Count); 
            Assert.Equal(data[0].Id, model[0].Id);
        }

    }
}
