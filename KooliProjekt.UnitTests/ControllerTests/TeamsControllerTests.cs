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
        [Fact]
        public async void Index_should_return_correct_view_and_data()
        {
            // Arrange
            var teamsServiceMock = new Mock<ITeamsService>();
            var controller = new TeamsController(teamsServiceMock.Object);

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

            teamsServiceMock.Setup(service => service.AllTeams(It.IsAny<TeamsSearch>())).ReturnsAsync(data);
            // Act
            var result = await controller.Index() as ViewResult;


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
    }
}
