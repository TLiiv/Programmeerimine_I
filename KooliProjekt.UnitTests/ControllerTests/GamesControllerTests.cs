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
        [Fact]
        public async void Index_should_return_correct_view_and_data()
        {
            // Arrange
            var gamesServiceMock = new Mock<IGamesService>();
            var controller = new GamesController(gamesServiceMock.Object);

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


            gamesServiceMock.Setup(service => service.AllGames(It.IsAny<GamesSearch>()))
                    .ReturnsAsync(data);

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            //viewname check
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
    }
}
