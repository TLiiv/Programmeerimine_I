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
    public class LeaderboardsControllerTests
    {
        [Fact]
        public async void Index_should_return_correct_view_and_data()
        {
            // Arrange
            var leaderboardsServiceMock = new Mock<ILeaderboardsService>();
            var controller = new LeaderboardsController(leaderboardsServiceMock.Object);

            var user = new User { UserId = Guid.NewGuid(), UserName = "Test User" };

            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Test Tournament",
                TournamentStartDate = DateTime.UtcNow,
                TournamentEndtDate = DateTime.UtcNow.AddMonths(1),
                Status = TournamentStatus.NotStarted,
                Format = TournamentFormat.SixteenTeams
            };

            var data = new List<Leaderboard>
            {
            new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                UserId = user.UserId,
                User = user, // Assign the User object
                TournamentId = tournament.TournamentId,
                Tournament = tournament, // Assign the Tournament object
                PredictedPoints = 20,
                Rank = 1
            }
            };

            leaderboardsServiceMock.Setup(service => service.AllLeaderboards()).ReturnsAsync(data);
            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            //viewname check
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));

            var model = result.Model as List<Leaderboard>;
            Assert.NotNull(model);
            Assert.Equal(data.Count, model.Count);

            // verify the first leaderboard entry
            Assert.Equal(data[0].LeaderBoardId, model[0].LeaderBoardId);
            Assert.Equal(data[0].PredictedPoints, model[0].PredictedPoints);
            Assert.Equal(data[0].Rank, model[0].Rank);
            Assert.Equal(data[0].User.UserName, model[0].User.UserName);
            Assert.Equal(data[0].Tournament.TournamentName, model[0].Tournament.TournamentName); 
        }
    }
}

