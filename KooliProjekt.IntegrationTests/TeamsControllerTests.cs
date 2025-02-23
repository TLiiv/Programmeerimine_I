using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            // Turn of Redirection trackin (for post mehtod status codes (after user save etc there is redirection))
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
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
        [InlineData("/Teams/Details/1")]
        [InlineData("/Teams/Edit/1")]
        [InlineData("/Teams/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_teams_id_is_missing_or_not_found(string url)
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

        //POST
        [Fact]
        public async Task Create_should_save_new_team()
        {
            // Arrange
            // Add form data
            var formValues = new Dictionary<string, string>
            {
                { "TeamName", "Team A" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Teams/Create", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently);

            // Check if the team was created
            var team = _context.Teams.FirstOrDefault(t => t.TeamName == "Team A");
            Assert.NotNull(team);
            Assert.NotEqual(Guid.Empty, team.TeamId);
            Assert.Equal("Team A", team.TeamName);
        }

        [Fact]
        public async Task Create_should_not_save_team_when_incorrect_data()
        {
            // Arrange
            // Add form data
            var formValues = new Dictionary<string, string>
            {
                { "TeamName", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Teams/Create", content);

            // Assert

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Users.Any());
        }



        [Fact]
        public async Task Edit_should_update_team()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "TeamName", "Team A" },
                { "GoalsScored", "10" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "3" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act - Create a new team
            using var response = await _client.PostAsync("/Teams/Create", content);

            // Retrieve team from DB
            var team = _context.Teams.FirstOrDefault();
            Assert.NotNull(team);

            // Detach to prevent tracking issues
            _context.Entry(team).State = EntityState.Detached;

            // Prepare edit form data
            var editFormValues = new Dictionary<string, string>
            {
                { "TeamId", team.TeamId.ToString() },
                { "TeamName", "Updated Team" },
                { "GoalsScored", "15" },
                { "GoalsScoredAgainstThem", "7" },
                { "GamesWon", "5" },
                { "GamesLost", "2" },
                { "GamesPlayed", "7" }
            };

            using var editContent = new FormUrlEncodedContent(editFormValues);

            // Act - Edit the team
            using var editResponse = await _client.PostAsync($"/Teams/Edit/{team.TeamId}", editContent);

            // Assert
            editResponse.EnsureSuccessStatusCode();

            // Check if the team was updated
            var updatedTeam = _context.Teams.FirstOrDefault(t => t.TeamId == team.TeamId);
            Assert.NotNull(updatedTeam);
            Assert.Equal("Updated Team", updatedTeam.TeamName);
            Assert.Equal(15, updatedTeam.GoalsScored);
        }
        [Fact]
        public async Task Edit_should_not_update_team_when_incorrect_data()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "TeamName", "Team A" },
                { "GoalsScored", "10" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "3" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act - Create a new team
            using var response = await _client.PostAsync("/Teams/Create", content);

            // Retrieve team from DB
            var team = _context.Teams.FirstOrDefault();
            Assert.NotNull(team);

            // Detach to prevent tracking issues
            _context.Entry(team).State = EntityState.Detached;

            // Prepare edit form data
            var editFormValues = new Dictionary<string, string>
            {
                { "TeamId", team.TeamId.ToString() },
                { "TeamName", "" },
                { "GoalsScored", "15" },
                { "GoalsScoredAgainstThem", "7" },
                { "GamesWon", "5" },
                { "GamesLost", "2" },
                { "GamesPlayed", "7" }
            };

            using var editContent = new FormUrlEncodedContent(editFormValues);

            // Act - Edit the team
            using var editResponse = await _client.PostAsync($"/Teams/Edit/{team.TeamId}", editContent);

            // Assert
            editResponse.EnsureSuccessStatusCode();

            // Check if the team was updated
            var updatedTeam = _context.Teams.FirstOrDefault(t => t.TeamId == team.TeamId);
            Assert.NotNull(updatedTeam);
            Assert.Equal("Team A", updatedTeam.TeamName);
            Assert.Equal(10, updatedTeam.GoalsScored);

        }
        [Fact]
        public async Task Delete_should_delete_team()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "TeamName", "Team A" },
                { "GoalsScored", "10" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "3" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act - Create a new team
            using var response = await _client.PostAsync("/Teams/Create", content);

            // Retrieve team from DB
            var team = _context.Teams.FirstOrDefault();
            Assert.NotNull(team);

            // Detach to prevent tracking issues
            _context.Entry(team).State = EntityState.Detached;

            // Act - Delete the team
            using var deleteResponse = await _client.PostAsync($"/Teams/Delete/{team.TeamId}", null);

            // Assert
            // Check for redirect after successful deletion
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.Redirect || deleteResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Ensure the team was deleted from DB
            var deletedTeam = _context.Teams.FirstOrDefault(t => t.TeamId == team.TeamId);
            Assert.Null(deletedTeam);
        }

    }
    

}
