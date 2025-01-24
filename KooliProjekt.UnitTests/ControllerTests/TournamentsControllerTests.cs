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
    public class TournamentsControllerTests
    {
        [Fact]
        public async void Index_should_return_correct_view_and_data()
        {
            // Arrange
            var tournamentsServiceMock = new Mock<ITournamentsService>();
            var controller = new TournamentsController(tournamentsServiceMock.Object);

            var data = new List<Tournament>
            {
                new Tournament
                {
                    TournamentId = Guid.NewGuid(),
                    TournamentName = "Test tournament",
                    Leaderboards = new List<Leaderboard> { new Leaderboard { LeaderBoardId = Guid.NewGuid() } },
                    Games = new List<Game> { new Game 
                    {
                        GamesId = Guid.NewGuid(), 
                        AreTeamsConfirmed=true, 
                        AwayTeamId=Guid.NewGuid(), 
                        AwayTeam = new Team 
                        { 
                            TeamId = Guid.NewGuid(), TeamName = "Team1" 
                        }, 
                        HomeTeamId=Guid.NewGuid(),
                        HomeTeam = new Team
                        {
                             TeamId = Guid.NewGuid(), TeamName = "Team2"
                        }
                    } 
                    },
                    Teams = new List<Team>
                    {
                        new Team { TeamId = Guid.NewGuid(), TeamName = "Team1" },
                        new Team { TeamId = Guid.NewGuid(), TeamName = "Team2" } 
                    },
                    TournamentStartDate = DateTime.UtcNow,
                    TournamentEndtDate = DateTime.UtcNow.AddMonths(1),
                    Status = TournamentStatus.NotStarted,
                    Format = TournamentFormat.SixteenTeams
                },
            };

            tournamentsServiceMock.Setup(service => service.AllTournaments()).ReturnsAsync(data);
            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            //viewname check
            var model = result.Model as List<Tournament>;
            Assert.NotNull(model);
            Assert.Equal(data.Count, model.Count);

            // Verify the first tournament
            Assert.Equal(data[0].TournamentId, model[0].TournamentId);
            Assert.Equal(data[0].TournamentName, model[0].TournamentName);
            Assert.Equal(data[0].Status, model[0].Status);
            //check if we can get teams name 
            var teams = model[0].Teams;
            Assert.NotNull(teams);  
            Assert.Contains(teams, t => t.TeamName == "Team1");  
            Assert.Contains(teams, t => t.TeamName == "Team2");  
        }
    }
}
