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
        private readonly Mock<IUserBetsService> _userBetsServiceMock;
        private readonly UserBetsController _controller;

        public UserBetsControllerTests()
        {
            _userBetsServiceMock = new Mock<IUserBetsService>();
            _controller = new UserBetsController(_userBetsServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_and_data()
        {
            // Arrange

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

            _userBetsServiceMock.Setup(service => service.AllUserBets()).ReturnsAsync(data);
            // Act
            var result = await _controller.Index() as ViewResult;

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
        [Fact]
        public async Task Create_Should_Redirect_To_Correct_View_On_Successful_UserBet_Creation()
        {
            //Act

            var bet = new UserBets
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                User = new User { UserId = Guid.NewGuid(), UserName = "Test User" },
                GameId = Guid.NewGuid(),
                Game = new Game { GamesId = Guid.NewGuid() },
                PredictedWinningTeamId = Guid.NewGuid(),
                PredictedWinningTeam = new Team { TeamId = Guid.NewGuid() },
                TournamentId = Guid.NewGuid(),
                Tournament = new Tournament { TournamentId = Guid.NewGuid() },
                PredictedHomeGoals = 1,
                PredictedAwayGoals = 2,
                AccountBalance = 100.0,
                BetAmount = 10.0,
                BetPlacedDate = DateTime.UtcNow
            };
            _userBetsServiceMock
                .Setup(service => service.Save(bet))
                .Returns(Task.CompletedTask)
                .Verifiable();
            // Assign
            var result = await _controller.Create(bet) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _userBetsServiceMock.VerifyAll(); //check that save method is used
        }
        [Fact]
        public async Task Create_Should_Stay_On_View_When_Model_Is_Not_Valid()
        {
            //Arrange
            var bet = new UserBets
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                User = new User { UserId = Guid.NewGuid(), UserName = "Test User" },
                GameId = Guid.NewGuid(),
                Game = new Game { GamesId = Guid.NewGuid() },
                PredictedWinningTeamId = Guid.NewGuid(),
                PredictedWinningTeam = new Team { TeamId = Guid.NewGuid() },
                TournamentId = Guid.NewGuid(),
                Tournament = new Tournament { TournamentId = Guid.NewGuid() },
                PredictedHomeGoals = 1,
                PredictedAwayGoals = 2,
                AccountBalance = 100.0,
                BetAmount = 10.0,
                BetPlacedDate = DateTime.UtcNow
            };

            _userBetsServiceMock
               .Setup(service => service.Save(bet))
               .Returns(Task.CompletedTask);

            _userBetsServiceMock
                .Setup(service => service.GetDropdownData())
                .ReturnsAsync((
                    new List<Game>(),
                    new List<Team>(),
                    new List<Tournament>(),
                    new List<User>()
                    ));


            //Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(bet) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Create" ||
                        string.IsNullOrEmpty(result.ViewName));
        }
        [Fact]
        public async Task Edit_Should_Return_View_With_Changed_UserBet_Info()
        {
            //Arrange
            var betId = Guid.NewGuid();
            var bet = new UserBets
            {
                Id = betId,
                UserId = Guid.NewGuid(),
                User = new User { UserId = Guid.NewGuid(), UserName = "Test User" },
                GameId = Guid.NewGuid(),
                Game = new Game { GamesId = Guid.NewGuid() },
                PredictedWinningTeamId = Guid.NewGuid(),
                PredictedWinningTeam = new Team { TeamId = Guid.NewGuid() },
                TournamentId = Guid.NewGuid(),
                Tournament = new Tournament { TournamentId = Guid.NewGuid() },
                PredictedHomeGoals = 1,
                PredictedAwayGoals = 2,
                AccountBalance = 100.0,
                BetAmount = 10.0,
                BetPlacedDate = DateTime.UtcNow
            };
            _userBetsServiceMock
                .Setup(service => service.Save(bet))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _userBetsServiceMock
                .Setup(service => service.GetDropdownData())
                .ReturnsAsync((
                    new List<Game>(),
                    new List<Team>(),
                    new List<Tournament>(),
                    new List<User>()
                    ));
            //Act
            var result = await _controller.Edit(betId,bet) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Edit" ||
                        string.IsNullOrEmpty(result.ViewName));
            _userBetsServiceMock.VerifyAll(); //check that save method is used
        }

        [Fact]
        public async Task Edit_Should_Return_Not_Found_When_Id_Is_Null()
        {

            // Arrange
            var invalidId = Guid.NewGuid();
            var bet = new UserBets
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                User = new User { UserId = Guid.NewGuid(), UserName = "Test User" },
                GameId = Guid.NewGuid(),
                Game = new Game { GamesId = Guid.NewGuid() },
                PredictedWinningTeamId = Guid.NewGuid(),
                PredictedWinningTeam = new Team { TeamId = Guid.NewGuid() },
                TournamentId = Guid.NewGuid(),
                Tournament = new Tournament { TournamentId = Guid.NewGuid() },
                PredictedHomeGoals = 1,
                PredictedAwayGoals = 2,
                AccountBalance = 100.0,
                BetAmount = 10.0,
                BetPlacedDate = DateTime.UtcNow
            };



            // Act
            var result = await _controller.Edit(invalidId, bet);


            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);

        }
        [Fact]
        public async Task Edit_Should_Return_Error_When_Model_Not_Valid()
        {
            //Arrange
            var bet = new UserBets
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                User = new User { UserId = Guid.NewGuid(), UserName = "Test User" },
                GameId = Guid.NewGuid(),
                Game = new Game { GamesId = Guid.NewGuid() },
                PredictedWinningTeamId = Guid.NewGuid(),
                PredictedWinningTeam = new Team { TeamId = Guid.NewGuid() },
                TournamentId = Guid.NewGuid(),
                Tournament = new Tournament { TournamentId = Guid.NewGuid() },
                PredictedHomeGoals = 1,
                PredictedAwayGoals = 2,
                AccountBalance = 100.0,
                BetAmount = 10.0,
                BetPlacedDate = DateTime.UtcNow
            };

            _userBetsServiceMock
                .Setup(service => service.Save(bet))
                .Returns(Task.CompletedTask);
            _userBetsServiceMock
                .Setup(service => service.GetDropdownData())
                .ReturnsAsync((
                    new List<Game>(),
                    new List<Team>(),
                    new List<Tournament>(),
                    new List<User>()
                    ));


            //Act
            _controller.ModelState.AddModelError("error", "error"); //Give error to model and
            var result = await _controller.Edit(bet.Id, bet) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.False(result.ViewData.ModelState.IsValid); //check if the error works and modelstate is false
        }

        [Fact]
        public async Task DeleteConfirmed_Should_Redirect_On_Correct_Id_And_Successful_Delete()
        {
            //Arrange
            var Id = Guid.NewGuid();
            _userBetsServiceMock
                .Setup(service => service.Delete(Id))
                .Returns(Task.CompletedTask)
                .Verifiable();
            //Act

            var result = await _controller.DeleteConfirmed(Id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _userBetsServiceMock.VerifyAll();
        }
    }
    }
