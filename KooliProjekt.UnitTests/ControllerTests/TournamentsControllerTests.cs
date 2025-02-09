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
        private readonly Mock<ITournamentsService> _tournamentsServiceMock;
        private readonly TournamentsController _controller;

        public TournamentsControllerTests() 
        {
            _tournamentsServiceMock = new Mock<ITournamentsService>();
            _controller = new TournamentsController(_tournamentsServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_and_data()
        {
            // Arrange
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

            _tournamentsServiceMock.Setup(service => service.AllTournaments()).ReturnsAsync(data);
            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            //viewname check
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));

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

        //POST
        [Fact]
        public async Task Create_Should_Redirect_To_Correct_View_On_Successful_Game_Creation()
        {
            //Act

            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship Tournament",
                TournamentStartDate = DateTime.Now,
                TournamentEndtDate = DateTime.Now.AddMonths(1),
                Status = TournamentStatus.Ongoing,
                Format = TournamentFormat.EightTeams
            };

            _tournamentsServiceMock
                .Setup(service => service.Save(tournament))
                .Returns(Task.CompletedTask)
                .Verifiable();
            // Assign
            var result = await _controller.Create(tournament) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _tournamentsServiceMock.VerifyAll(); //check that save method is used
        }

        [Fact]
        public async Task Create_Should_Stay_On_View_When_Model_Is_Not_Valid()
        {
            //Arrange
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship Tournament",
                TournamentStartDate = DateTime.Now,
                TournamentEndtDate = DateTime.Now.AddMonths(1),
                Status = TournamentStatus.Ongoing,
                Format = TournamentFormat.EightTeams
            };

            _tournamentsServiceMock
               .Setup(service => service.Save(tournament))
               .Returns(Task.CompletedTask);



            //Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(tournament) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Create" ||
                        string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public async Task Edit_Should_Redirect_When_Game_Info_Is_Changed()
        {
            //Arrange
            var tournamentId = Guid.NewGuid();
            var tournament = new Tournament
            {
                TournamentId = tournamentId,
                TournamentName = "Championship Tournament",
                TournamentStartDate = DateTime.Now,
                TournamentEndtDate = DateTime.Now.AddMonths(1),
                Status = TournamentStatus.Ongoing,
                Format = TournamentFormat.EightTeams
            };

            _tournamentsServiceMock
                .Setup(service => service.Save(tournament))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            var result = await _controller.Edit(tournamentId, tournament) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _tournamentsServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_Should_Return_Not_Found_When_Incorrect_Id()
        {
            //Arrange
            var incorrectId = Guid.NewGuid();
            var tournamentId = Guid.NewGuid();
            var tournament = new Tournament
            {
                TournamentId = tournamentId,
            };

            //Act
            var result = await _controller.Edit(incorrectId, tournament);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Should_Return_Error_When_Model_Not_Valid_And_Return_Correct_View()
        {
            //Arrange

            var tournamentId = Guid.NewGuid();
            var tournament = new Tournament
            {
                TournamentId = tournamentId,
                TournamentName = "Championship Tournament",
                TournamentStartDate = DateTime.Now,
                TournamentEndtDate = DateTime.Now.AddMonths(1),
                Status = TournamentStatus.Ongoing,
                Format = TournamentFormat.EightTeams
            };



            //Act
            _controller.ModelState.AddModelError("error", "error"); //Give error to model and
            var result = await _controller.Edit(tournamentId,tournament) as ViewResult;

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
            _tournamentsServiceMock
                .Setup(service => service.Delete(Id))
                .Returns(Task.CompletedTask)
                .Verifiable();
            //Act

            var result = await _controller.DeleteConfirmed(Id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _tournamentsServiceMock.VerifyAll();
        }

    }
}
