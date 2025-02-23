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
    public class LeaderboardsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public LeaderboardsControllerTests()
        {
            // Turn of Redirection trackin (for post mehtod status codes (after user save etc there is redirection))
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));

        }
        //GET METHODS
        [Theory]
        [InlineData("/Leaderboards")]
        [InlineData("/Leaderboards/Create")]
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
        [InlineData("/Leaderboards/Details/")]
        [InlineData("/Leaderboards/Edit/")]
        [InlineData("/Leaderboards/Delete/")]
        [InlineData("/Leaderboards/Details/1")]
        [InlineData("/Leaderboards/Edit/1")]
        [InlineData("/Leaderboards/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_leaderboards_id_is_missing_or_not_found(string url)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

      

        [Fact]
        public async Task Details_should_return_succsess_when_leaderboards_is_found()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "user1",
                FirstName = "First",
                LastName = "User",
                Email = "user1@example.com",
                Password = "asd",
                PhoneNumber = "51231231",
                IsAdmin = false
            };
            _context.Users.Add(user);

            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship"
            };
            _context.Tournaments.Add(tournament);

            var leaderboard = new Leaderboard
            {
                UserId = user.UserId,
                TournamentId = tournament.TournamentId,
                PredictedPoints = 50,
                Rank = 1
            };
            _context.Leaderboards.Add(leaderboard);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Leaderboards/Details/" + leaderboard.LeaderBoardId);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Edit_should_return_succsess_when_leaderboards_is_found()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "user1",
                FirstName = "First",
                LastName = "User",
                Email = "user1@example.com",
                Password = "asd",
                PhoneNumber = "51231231",
                IsAdmin = false
            };
            _context.Users.Add(user);

            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship"
            };
            _context.Tournaments.Add(tournament);

            var leaderboard = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                UserId = user.UserId,
                TournamentId = tournament.TournamentId,
                PredictedPoints = 50,
                Rank = 1
            };
            _context.Leaderboards.Add(leaderboard);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Leaderboards/Edit/" + leaderboard.LeaderBoardId);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Delete_should_return_succsess_when_leaderboards_is_found()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "user1",
                FirstName = "First",
                LastName = "User",
                Email = "user1@example.com",
                Password = "asd",
                PhoneNumber = "51231231",
                IsAdmin = false
            };
            _context.Users.Add(user);

            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship"
            };
            _context.Tournaments.Add(tournament);

            var leaderboard = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                UserId = user.UserId,
                TournamentId = tournament.TournamentId,
                PredictedPoints = 50,
                Rank = 1
            };
            _context.Leaderboards.Add(leaderboard);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Leaderboards/Delete/" + leaderboard.LeaderBoardId);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        //POST
        [Fact]
        public async Task Create_should_save_new_leaderboard()
        {
            // Arrange
            //Create user
            var userFormValues = new Dictionary<string, string>
            {
                { "UserName", "user1"},
                { "FirstName", "First"},
                { "LastName", "User"},
                { "Email", "user1@example.com"},
                { "Password", "asd"},
                { "PhoneNumber", "51231231"},
                { "IsAdmin", "true"},
            };

            using var content = new FormUrlEncodedContent(userFormValues);

            //Act
            using var response = await _client.PostAsync("/Users/Create", content);


            // Retrieve user from DB
            var user = _context.Users.FirstOrDefault();
            Assert.NotNull(user);

            // Create a new tournament
            var tournamentFormValues = new Dictionary<string, string>
            {
                { "TournamentName", "Champions Cup" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var tournamentContent = new FormUrlEncodedContent(tournamentFormValues);
            using var tournamentResponse = await _client.PostAsync("/Tournaments/Create", tournamentContent);

            var tournament = _context.Tournaments.FirstOrDefault();
            Assert.NotNull(tournament);

            //Create a new leaderboard
            var leaderboardsFormValues = new Dictionary<string, string>
            {
                { "UserId", user.UserId.ToString() },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "PredictedPoints", "4" },  
                { "Rank", "1" }  
            };

            using var leaderboardsContent = new FormUrlEncodedContent(leaderboardsFormValues);

            // Act - Create the game
            using var leaderboardsResponse = await _client.PostAsync("/Leaderboards/Create", leaderboardsContent);

            // Assert - Check redirection
            Assert.True(leaderboardsResponse.StatusCode == HttpStatusCode.Redirect || leaderboardsResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Check if the game was created
            var leaderboard = _context.Leaderboards.FirstOrDefault();
            Assert.NotNull(leaderboard);
            Assert.Equal(tournament.TournamentId, leaderboard.TournamentId);
            Assert.Equal(user.UserId, leaderboard.UserId);
            Assert.Equal(4, leaderboard.PredictedPoints);

        }
        [Fact]
        public async Task Create_should_not_save_new_leaderboard_with_incorrect_data()
        {
            // Arrange
            //Create user
            var userFormValues = new Dictionary<string, string>
            {
                { "UserName", "user1"},
                { "FirstName", "First"},
                { "LastName", "User"},
                { "Email", "user1@example.com"},
                { "Password", "asd"},
                { "PhoneNumber", "51231231"},
                { "IsAdmin", "true"},
            };

            using var content = new FormUrlEncodedContent(userFormValues);

            //Act
            using var response = await _client.PostAsync("/Users/Create", content);


            // Retrieve user from DB
            var user = _context.Users.FirstOrDefault();
            Assert.NotNull(user);

            // Create a new tournament
            var tournamentFormValues = new Dictionary<string, string>
            {
                { "TournamentName", "Champions Cup" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var tournamentContent = new FormUrlEncodedContent(tournamentFormValues);
            using var tournamentResponse = await _client.PostAsync("/Tournaments/Create", tournamentContent);

            var tournament = _context.Tournaments.FirstOrDefault();
            Assert.NotNull(tournament);

            //Create a new leaderboard
            var leaderboardsFormValues = new Dictionary<string, string>
            {
                { "UserId", "wrong" },
                { "TournamentId", "wrong" },
                { "PredictedPoints", "4" },
                { "Rank", "1" }
            };

            using var leaderboardsContent = new FormUrlEncodedContent(leaderboardsFormValues);

            // Act - Create the leaderboard
            using var leaderboardsResponse = await _client.PostAsync("/Leaderboards/Create", leaderboardsContent);

            // Assert - Check redirection
            leaderboardsResponse.EnsureSuccessStatusCode();
            Assert.False(_context.Leaderboards.Any());
        }

        [Fact]
        public async Task Edit_should_update_leaderboard()
        {
            // Arrange
            // Create a new user
            var userFormValues = new Dictionary<string, string>
            {
                { "UserName", "user1"},
                { "FirstName", "First"},
                { "LastName", "User"},
                { "Email", "user1@example.com"},
                { "Password", "asd"},
                { "PhoneNumber", "51231231"},
                { "IsAdmin", "true"},
            };

            using var userContent = new FormUrlEncodedContent(userFormValues);
            using var userResponse = await _client.PostAsync("/Users/Create", userContent);

            var user = _context.Users.FirstOrDefault(u => u.UserName == "user1");
            Assert.NotNull(user);

            // Create a new tournament
            var tournamentFormValues = new Dictionary<string, string>
            {
                { "TournamentName", "Champions Cup" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var tournamentContent = new FormUrlEncodedContent(tournamentFormValues);
            using var tournamentResponse = await _client.PostAsync("/Tournaments/Create", tournamentContent);

            var tournament = _context.Tournaments.FirstOrDefault(t => t.TournamentName == "Champions Cup");
            Assert.NotNull(tournament);

            // Create a new leaderboard entry
            var leaderboardsFormValues = new Dictionary<string, string>
            {
                { "UserId", user.UserId.ToString() },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "PredictedPoints", "4" },
                { "Rank", "1" }
            };

            using var leaderboardsContent = new FormUrlEncodedContent(leaderboardsFormValues);
            using var leaderboardsResponse = await _client.PostAsync("/Leaderboards/Create", leaderboardsContent);

            var leaderboard = _context.Leaderboards.FirstOrDefault(l => l.UserId == user.UserId);
            Assert.NotNull(leaderboard);

            // Detach the leaderboard entry to prevent tracking issues
            _context.Entry(leaderboard).State = EntityState.Detached;

            // Prepare edit form data
            var editFormValues = new Dictionary<string, string>
            {
                { "LeaderBoardId", leaderboard.LeaderBoardId.ToString() },
                { "UserId", user.UserId.ToString() },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "PredictedPoints", "10" },  // Updated points
                { "Rank", "2" }               // Updated rank
            };

            // Act
            using var editContent = new FormUrlEncodedContent(editFormValues);
            using var editResponse = await _client.PostAsync($"/Leaderboards/Edit/{leaderboard.LeaderBoardId}", editContent);

            // Assert
            Assert.True(editResponse.StatusCode == HttpStatusCode.Redirect || editResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Verify the update
            var updatedLeaderboard = _context.Leaderboards.FirstOrDefault(l => l.LeaderBoardId == leaderboard.LeaderBoardId);
            Assert.NotNull(updatedLeaderboard);
            Assert.Equal(10, updatedLeaderboard.PredictedPoints);
            Assert.Equal(2, updatedLeaderboard.Rank);
        }
        [Fact]
        public async Task Edit_should_not_update_leaderboard_when_incorrect_data()
        {
            // Arrange
            // Create a new user
            var userFormValues = new Dictionary<string, string>
            {
                { "UserName", "user1"},
                { "FirstName", "First"},
                { "LastName", "User"},
                { "Email", "user1@example.com"},
                { "Password", "asd"},
                { "PhoneNumber", "51231231"},
                { "IsAdmin", "true"},
            };

            using var userContent = new FormUrlEncodedContent(userFormValues);
            using var userResponse = await _client.PostAsync("/Users/Create", userContent);

            var user = _context.Users.FirstOrDefault(u => u.UserName == "user1");
            Assert.NotNull(user);

            // Create a new tournament
            var tournamentFormValues = new Dictionary<string, string>
            {
                { "TournamentName", "Champions Cup" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var tournamentContent = new FormUrlEncodedContent(tournamentFormValues);
            using var tournamentResponse = await _client.PostAsync("/Tournaments/Create", tournamentContent);

            var tournament = _context.Tournaments.FirstOrDefault(t => t.TournamentName == "Champions Cup");
            Assert.NotNull(tournament);

            // Create a new leaderboard entry
            var leaderboardFormValues = new Dictionary<string, string>
            {
                { "UserId", user.UserId.ToString() },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "PredictedPoints", "4" },
                { "Rank", "1" }
            };

            using var leaderboardContent = new FormUrlEncodedContent(leaderboardFormValues);
            using var leaderboardResponse = await _client.PostAsync("/Leaderboards/Create", leaderboardContent);

            var leaderboard = _context.Leaderboards.FirstOrDefault(l => l.UserId == user.UserId);
            Assert.NotNull(leaderboard);

            // Detach the leaderboard entry to prevent tracking issues
            _context.Entry(leaderboard).State = EntityState.Detached;

            // Prepare incorrect edit form data
            var editFormValues = new Dictionary<string, string>
             {
                 { "LeaderBoardId", leaderboard.LeaderBoardId.ToString() },
                 { "UserId", user.UserId.ToString() },
                 { "TournamentId", tournament.TournamentId.ToString() },
                 { "PredictedPoints", "-10" },  // Invalid negative points
                 { "Rank", "invalid_rank" }     // Invalid rank (should be numeric)
             };

            // Act
            using var editContent = new FormUrlEncodedContent(editFormValues);
            using var editResponse = await _client.PostAsync($"/Leaderboards/Edit/{leaderboard.LeaderBoardId}", editContent);

            // Assert that request did not succeed
            editResponse.EnsureSuccessStatusCode();

            // Verify the leaderboard remains unchanged
            var updatedLeaderboard = _context.Leaderboards.FirstOrDefault(l => l.LeaderBoardId == leaderboard.LeaderBoardId);
            Assert.NotNull(updatedLeaderboard);
            Assert.Equal(4, updatedLeaderboard.PredictedPoints);  // Should remain unchanged
            Assert.Equal(1, updatedLeaderboard.Rank);  // Should remain unchanged
        }
        [Fact]
        public async Task Delete_should_delete_leaderboard()
        {
            // Arrange
            // Create a new user
            var userFormValues = new Dictionary<string, string>
            {
                { "UserName", "user1"},
                { "FirstName", "First"},
                { "LastName", "User"},
                { "Email", "user1@example.com"},
                { "Password", "asd"},
                { "PhoneNumber", "51231231"},
                { "IsAdmin", "true"},
            };

            using var userContent = new FormUrlEncodedContent(userFormValues);
            using var userResponse = await _client.PostAsync("/Users/Create", userContent);

            var user = _context.Users.FirstOrDefault();
            Assert.NotNull(user);

            // Create a new tournament
            var tournamentFormValues = new Dictionary<string, string>
            {
                { "TournamentName", "Champions Cup" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var tournamentContent = new FormUrlEncodedContent(tournamentFormValues);
            using var tournamentResponse = await _client.PostAsync("/Tournaments/Create", tournamentContent);

            var tournament = _context.Tournaments.FirstOrDefault();
            Assert.NotNull(tournament);

            // Create a new leaderboard entry
            var leaderboardsFormValues = new Dictionary<string, string>
            {
                { "UserId", user.UserId.ToString() },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "PredictedPoints", "4" },
                { "Rank", "1" }
            };

            using var leaderboardsContent = new FormUrlEncodedContent(leaderboardsFormValues);
            using var leaderboardsResponse = await _client.PostAsync("/Leaderboards/Create", leaderboardsContent);

            var leaderboard = _context.Leaderboards.FirstOrDefault();
            Assert.NotNull(leaderboard);

            // Detach the leaderboard entry to prevent tracking issues
            _context.Entry(leaderboard).State = EntityState.Detached;

            // Act 
            using var deleteResponse = await _client.PostAsync($"/Leaderboards/Delete/{leaderboard.LeaderBoardId}", null);

            // Assert
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.Redirect || deleteResponse.StatusCode == HttpStatusCode.MovedPermanently);
            var deletedLeaderboard = _context.Leaderboards.FirstOrDefault(l => l.LeaderBoardId == leaderboard.LeaderBoardId);
            Assert.Null(deletedLeaderboard);
        }
    }
}
