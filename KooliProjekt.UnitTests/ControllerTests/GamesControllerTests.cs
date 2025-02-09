using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KooliProjekt.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Xunit;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Moq;
using KooliProjekt.Search;

namespace KooliProjekt.UnitTests.ControllerTests

{
    public class GamesControllerTests
    {
        private readonly Mock<IGamesService> _gamesServiceMock;
        private readonly GamesController _controller;
        
        public GamesControllerTests() 
        {
            _gamesServiceMock = new Mock<IGamesService>();
            _controller = new GamesController(_gamesServiceMock.Object);
        }
        [Fact]
        public async Task Index_should_return_correct_view_and_data()
        {
            // Arrange
           
            var homeTeam = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Home Team"
            };

            var awayTeam = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Away Team"
            };

            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Test Tournament",
                TournamentStartDate = DateTime.UtcNow,
                TournamentEndtDate = DateTime.UtcNow.AddMonths(1),
                Status = TournamentStatus.NotStarted,
                Format = TournamentFormat.SixteenTeams
            };

            var data = new List<Game>
            {
                new Game
                {
                    GamesId = Guid.NewGuid(),
                    GameStartDate = DateTime.UtcNow.AddDays(1), 
                    GameStartTime = DateTime.UtcNow.AddHours(2), 
                    HomeTeamId = homeTeam.TeamId,
                    AwayTeamId = awayTeam.TeamId,
                    HomeTeam = homeTeam, 
                    AwayTeam = awayTeam, 
                    AreTeamsConfirmed = true,
                    TournamentId = tournament.TournamentId,
                    Tournament = tournament 
                }
            };


            _gamesServiceMock.Setup(service => service.AllGames(It.IsAny<GamesSearch>()))
                    .ReturnsAsync(data);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
           
            //viewname check
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));

            var model = result.Model as List<Game>;
            Assert.NotNull(model);
            Assert.Equal(data.Count, model.Count);

            // Verify the first game
            Assert.Equal(data[0].GamesId, model[0].GamesId);
            Assert.Equal(data[0].GameStartDate, model[0].GameStartDate);
            Assert.Equal(data[0].GameStartTime, model[0].GameStartTime);
            Assert.Equal(data[0].HomeTeamId, model[0].HomeTeamId);
            Assert.Equal(data[0].AwayTeamId, model[0].AwayTeamId);
            Assert.Equal(data[0].AreTeamsConfirmed, model[0].AreTeamsConfirmed);
            Assert.Equal(data[0].TournamentId, model[0].TournamentId);
            
            //check if we can get correct teams name 
            Assert.Equal(data[0].HomeTeam.TeamName, model[0].HomeTeam.TeamName);
            Assert.Equal(data[0].AwayTeam.TeamName, model[0].AwayTeam.TeamName);
        }
        [Fact]
        public async Task Create_Should_Redirect_To_Correct_View_On_Successful_Game_Creation()
        {
            //Act

            var game = new Game
                    {
                GamesId = Guid.NewGuid(),
                GameStartDate = DateTime.Now,
                GameStartTime = DateTime.Now,
                HomeTeamId = Guid.NewGuid(),
                AwayTeamId = Guid.NewGuid(),
                AreTeamsConfirmed = true,
                TournamentId = Guid.NewGuid()
            };
            _gamesServiceMock
                .Setup(service => service.Save(game))
                .Returns(Task.CompletedTask)
                .Verifiable();
            // Assign
            var result = await _controller.Create(game) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _gamesServiceMock.VerifyAll(); //check that save method is used
        }

        [Fact]
        public async Task Create_Should_Stay_On_View_When_Model_Is_Not_Valid()
        {
            //Arrange
            var game = new Game
            {
                GamesId = Guid.NewGuid(),
                GameStartDate = DateTime.Now,
                GameStartTime = DateTime.Now,
                HomeTeamId = Guid.NewGuid(),
                AwayTeamId = Guid.NewGuid(),
                AreTeamsConfirmed = true,
                TournamentId = Guid.NewGuid()
            };

            _gamesServiceMock
               .Setup(service => service.Save(game))
               .Returns(Task.CompletedTask);

            _gamesServiceMock
                .Setup(service => service.GetDropdownData())
                .ReturnsAsync((Teams: new List<Team>
                 {
                     new Team { TeamId = Guid.NewGuid(), TeamName = "Team A" },
                     new Team { TeamId = Guid.NewGuid(), TeamName = "Team B" }
                 },
                 Tournaments: new List<Tournament>
                 {
                     new Tournament { TournamentId = Guid.NewGuid(), TournamentName = "Tournament X" },
                     new Tournament { TournamentId = Guid.NewGuid(), TournamentName = "Tournament Y" }
                 }));



            //Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(game) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Create" ||
                        string.IsNullOrEmpty(result.ViewName));
        }

      
        [Fact]
        public async Task Edit_Should_Redirect_When_Game_Info_Is_Changed()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                GamesId = gameId,
                GameStartDate = DateTime.Now,
                GameStartTime = DateTime.Now,
                HomeTeamId = Guid.NewGuid(),
                AwayTeamId = Guid.NewGuid(),
                AreTeamsConfirmed = true,
                TournamentId = Guid.NewGuid()
            };

            _gamesServiceMock
                .Setup(service => service.Save(game))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            var result = await _controller.Edit(gameId,game) as RedirectToActionResult;
 
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _gamesServiceMock.VerifyAll();
        }
        [Fact]
        public async Task Edit_Should_Return_Not_Found_When_Incorrect_Id()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            var incorrectId = Guid.NewGuid();
            var game = new Game
            {
                GamesId = gameId,
            };
            //Act
            var result = await _controller.Edit(incorrectId,game);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Edit_Should_Return_Error_When_Model_Not_Valid_And_Return_Correct_View()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            var homeTeamId = Guid.NewGuid();
            var awayTeamId = Guid.NewGuid();
            var tournamentId = Guid.NewGuid();
            
            var game = new Game
            {
                GamesId = gameId,
                GameStartDate = DateTime.Now,
                GameStartTime = DateTime.Now,
                HomeTeamId = Guid.NewGuid(),
                AwayTeamId = Guid.NewGuid(),
                AreTeamsConfirmed = true,
                TournamentId = Guid.NewGuid()
            };
            
            //dropdown data 
            var teams = new List<Team>
            {
                new Team { TeamId = homeTeamId, TeamName = "Home Team" },
                new Team { TeamId = awayTeamId, TeamName = "Away Team" }
            };

                    var tournaments = new List<Tournament>
            {
                new Tournament { TournamentId = tournamentId, TournamentName = "Test Tournament" }
            };



            _gamesServiceMock
                .Setup(service => service.GetDropdownData())
                .ReturnsAsync((teams, tournaments));


            //Act
            _controller.ModelState.AddModelError("error", "error"); //Give error to model and
            var result = await _controller.Edit(game.GamesId, game) as ViewResult;

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
            _gamesServiceMock
                .Setup(service => service.Delete(Id))
                .Returns(Task.CompletedTask)
                .Verifiable();
            //Act

            var result = await _controller.DeleteConfirmed(Id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _gamesServiceMock.VerifyAll();
        }
    }
}
