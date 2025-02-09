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
        private readonly Mock<ILeaderboardsService> _leaderboardsServiceMock;
        private readonly LeaderboardsController _controller;

        public LeaderboardsControllerTests()
        {
            _leaderboardsServiceMock = new Mock<ILeaderboardsService>();
            _controller = new LeaderboardsController(_leaderboardsServiceMock.Object);
        }
        //GET
        [Fact]
        public async Task Index_should_return_correct_view_and_data()
        {
            // Arrange

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

            _leaderboardsServiceMock.Setup(service => service.AllLeaderboards()).ReturnsAsync(data);
            // Act
            var result = await _controller.Index() as ViewResult;

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
        //POST
        [Fact]
        public async Task Create_Should_Redirect_To_Correct_View_On_Successful_Leaderboard_Creation()
        {
            //Act

            var leaderboard = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TournamentId = Guid.NewGuid(),
                PredictedPoints = 10,
                Rank = 1
            };
            _leaderboardsServiceMock
                .Setup(service => service.Save(leaderboard))
                .Returns(Task.CompletedTask)
                .Verifiable();
            // Assign
            var result = await _controller.Create(leaderboard) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _leaderboardsServiceMock.VerifyAll(); //check that save method is used
        }
        [Fact]
        public async Task Create_Should_Stay_On_View_When_Model_Is_Not_Valid()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var tournamentId = Guid.NewGuid();

            var leaderboard = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TournamentId = Guid.NewGuid(),
                PredictedPoints = 10,
                Rank = 1
            };

            var users = new List<User>
            {
                new User { UserId = userId, Email = "test@example.com" }
            };

                    var tournaments = new List<Tournament>
            {
                new Tournament { TournamentId = tournamentId, TournamentName = "Test Tournament" }
            };


            _leaderboardsServiceMock
               .Setup(service => service.Save(leaderboard))
               .Returns(Task.CompletedTask);

            _leaderboardsServiceMock
                .Setup(service => service.GetDropdownData())
                .ReturnsAsync((tournaments, users));

            //Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(leaderboard) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Create" ||
                        string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public async Task Edit_Should_Redirect_When_Game_Info_Is_Changed()
        {
            //Arrange
           
            var leaderboard = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TournamentId = Guid.NewGuid(),
                PredictedPoints = 10,
                Rank = 1
            };

            _leaderboardsServiceMock
                .Setup(service => service.Save(leaderboard))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            var result = await _controller.Edit(leaderboard.LeaderBoardId, leaderboard) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _leaderboardsServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_Should_Return_Not_Found_When_Incorrect_Id()
        {
            //Arrange
            var leaderboardId = Guid.NewGuid();
            var incorrectId = Guid.NewGuid();
            var leaderboard = new Leaderboard
            {
                LeaderBoardId = leaderboardId
            };
            //Act
            var result = await _controller.Edit(incorrectId, leaderboard);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Should_Return_Error_When_Model_Not_Valid_And_Return_Correct_View()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var tournamentId = Guid.NewGuid();

            var leaderboard = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TournamentId = Guid.NewGuid(),
                PredictedPoints = 10,
                Rank = 1
            };

            var users = new List<User>
            {
                new User { UserId = userId, Email = "test@example.com" }
            };

            var tournaments = new List<Tournament>
            {
                new Tournament { TournamentId = tournamentId, TournamentName = "Test Tournament" }
            };


            _leaderboardsServiceMock
                .Setup(service => service.GetDropdownData())
                .ReturnsAsync((tournaments, users));


            //Act
            _controller.ModelState.AddModelError("error", "error"); //Give error to model and
            var result = await _controller.Edit(leaderboard.LeaderBoardId, leaderboard) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.False(result.ViewData.ModelState.IsValid); //check if the error works and modelstate is false
            Assert.True(result.ViewName == "Create" ||
                       string.IsNullOrEmpty(result.ViewName));

        }

        [Fact]
        public async Task DeleteConfirmed_Should_Redirect_On_Correct_Id_And_Successful_Delete()
        {
            //Arrange
            var Id = Guid.NewGuid();
            _leaderboardsServiceMock
                .Setup(service => service.Delete(Id))
                .Returns(Task.CompletedTask)
                .Verifiable();
            //Act

            var result = await _controller.DeleteConfirmed(Id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _leaderboardsServiceMock.VerifyAll();
        }
    }
}

