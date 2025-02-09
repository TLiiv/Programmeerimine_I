using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TeamsControllerTests
    {
        private readonly Mock<ITeamsService> _teamsServiceMock;
        private readonly TeamsController _controller;
        public TeamsControllerTests() 
        {
            _teamsServiceMock = new Mock<ITeamsService>();
            _controller = new TeamsController(_teamsServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_and_data()
        {
            // Arrange

            var data = new List<Team>
            {
                new Team
                {
                    TeamId = Guid.NewGuid(),
                    TeamName = "Test team1",
                    GoalsScored = 3,
                    GoalsScoredAgainstThem = 4,
                    GamesWon = 1,
                    GamesLost = 2,
                    GamesPlayed = 3,
                },  
                new Team
                {
                    TeamId = Guid.NewGuid(),
                    TeamName = "Test team1",
                    GoalsScored = 3,
                    GoalsScoredAgainstThem = 4,
                    GamesWon = 1,
                    GamesLost = 2,
                    GamesPlayed = 3,
                },
            };

            _teamsServiceMock.Setup(service => service.AllTeams(It.IsAny<TeamsSearch>())).ReturnsAsync(data);
            // Act
            var result = await _controller.Index() as ViewResult;


            // Assert

            //viewname check
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));


            var model = result.Model as List<Team>;
            Assert.NotNull(model);
            Assert.Equal(data.Count, model.Count);

            // Verify the first team
            Assert.Equal(data[0].TeamId, model[0].TeamId);
            Assert.Equal(data[0].TeamName, model[0].TeamName);
            Assert.Equal(data[0].GoalsScored, model[0].GoalsScored);
            Assert.Equal(data[0].GoalsScoredAgainstThem, model[0].GoalsScoredAgainstThem);
            Assert.Equal(data[0].GamesWon, model[0].GamesWon);
            Assert.Equal(data[0].GamesLost, model[0].GamesLost);
            Assert.Equal(data[0].GamesPlayed, model[0].GamesPlayed);
        }
        //POST
        [Fact]
        public async Task Create_Should_Redirect_To_Correct_View_On_Successful_Game_Creation()
        {
            //Act

            var team = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Test Team",
                GoalsScored = 20,
                GoalsScoredAgainstThem = 10,
                GamesWon = 5,
                GamesLost = 2,
                GamesPlayed = 7
            };

            _teamsServiceMock
                .Setup(service => service.Save(team))
                .Returns(Task.CompletedTask)
                .Verifiable();
            // Assign
            var result = await _controller.Create(team) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _teamsServiceMock.VerifyAll(); //check that save method is used
        }
        [Fact]
        public async Task Create_Should_Stay_On_View_When_Model_Is_Not_Valid()
        {
            //Arrange
            var team = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Test Team",
                GoalsScored = 20,
                GoalsScoredAgainstThem = 10,
                GamesWon = 5,
                GamesLost = 2,
                GamesPlayed = 7
            };

            _teamsServiceMock
               .Setup(service => service.Save(team))
               .Returns(Task.CompletedTask);



            //Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(team) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Create" ||
                        string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public async Task Edit_Should_Redirect_When_Game_Info_Is_Changed()
        {
            //Arrange
            var teamId = Guid.NewGuid();
            var team = new Team
            {
                TeamId =teamId,
                TeamName = "Test Team",
                GoalsScored = 20,
                GoalsScoredAgainstThem = 10,
                GamesWon = 5,
                GamesLost = 2,
                GamesPlayed = 7
            };

            _teamsServiceMock
                .Setup(service => service.Save(team))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            var result = await _controller.Edit(teamId, team) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _teamsServiceMock.VerifyAll();
        }
        [Fact]
        public async Task Edit_Should_Return_Not_Found_When_Incorrect_Id()
        {
            //Arrange
            var teamId = Guid.NewGuid();
            var incorrectId = Guid.NewGuid();
            var team = new Team
            {
               TeamId = teamId,
            };
            //Act
            var result = await _controller.Edit(incorrectId, team);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task Edit_Should_Return_Error_When_Model_Not_Valid_And_Return_Correct_View()
        {
            //Arrange

            var teamId = Guid.NewGuid();
            var team = new Team
            {
                TeamId = teamId,
                TeamName = "Test Team",
                GoalsScored = 20,
                GoalsScoredAgainstThem = 10,
                GamesWon = 5,
                GamesLost = 2,
                GamesPlayed = 7
            };
  


            //Act
            _controller.ModelState.AddModelError("error", "error"); //Give error to model and
            var result = await _controller.Edit(teamId, team) as ViewResult;

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
            _teamsServiceMock
                .Setup(service => service.Delete(Id))
                .Returns(Task.CompletedTask)
                .Verifiable();
            //Act

            var result = await _controller.DeleteConfirmed(Id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            _teamsServiceMock.VerifyAll();
        }

    }
}
