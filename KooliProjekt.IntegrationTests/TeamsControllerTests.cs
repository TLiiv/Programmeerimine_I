using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TeamsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public TeamsControllerTests()
        {
            _client = Factory.CreateClient();
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));

        }
        //GET METHODS
        [Theory]
        [InlineData("/Teams")]
        [InlineData("/Teams/Create")]
        public async Task Get_Endpoints_Return_Success_And_Correct_Content_Type(string url)
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Teams/Details/")]
        [InlineData("/Teams/Edit/")]
        [InlineData("/Teams/Delete/")]
        public async Task Get_endpoints_should_return_not_found_when_bet_id_is_missing(string url)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/Teams/Details/1")]
        [InlineData("/Teams/Edit/1")]
        [InlineData("/Teams/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_bet_does_not_exist(string url)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Details_should_return_success_when_team_is_found()
        {
            // Arrange
            var team = new Team
            {
                TeamName = "Home Team",
                GoalsScored = 10,
                GoalsScoredAgainstThem = 5,
                GamesWon = 6,
                GamesLost = 3,
                GamesPlayed = 9
            };
            _context.Teams.Add(team);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Teams/Details/{team.TeamId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Edit_should_return_success_when_team_is_found()
        {
            // Arrange
            var team = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Home Team",
                GoalsScored = 10,
                GoalsScoredAgainstThem = 5,
                GamesWon = 6,
                GamesLost = 3,
                GamesPlayed = 9
            };
            _context.Teams.Add(team);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Teams/Edit/{team.TeamId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Delete_should_return_success_when_team_is_found()
        {
            // Arrange
            var team = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Home Team",
                GoalsScored = 10,
                GoalsScoredAgainstThem = 5,
                GamesWon = 6,
                GamesLost = 3,
                GamesPlayed = 9
            };
            _context.Teams.Add(team);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Teams/Delete/{team.TeamId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }

}
