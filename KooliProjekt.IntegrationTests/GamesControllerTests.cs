using Azure;
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
    public class GamesControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public GamesControllerTests()
        {
            // Turn of Redirection trackin (for post mehtod status codes (after user save etc there is redirection))
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));

        }
        //GET METHODS
        [Theory]
        [InlineData("/Games")]
        [InlineData("/Games/Create")]
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
        [InlineData("/Games/Details/")]
        [InlineData("/Games/Edit/")]
        [InlineData("/Games/Delete/")]
        [InlineData("/Games/Details/1")]
        [InlineData("/Games/Edit/1")]
        [InlineData("/Games/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_bet_id_is_missing_or_not_found(string url)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        
        [Fact]
        public async Task Details_should_return_succsess_when_game_is_found()
        {
                // Arrange
                var tournament = new Tournament
                {
                    TournamentId = Guid.NewGuid(),
                    TournamentName = "Championship"
                };
                _context.Tournaments.Add(tournament);

                var team1 = new Team
                {
                    TeamId = Guid.NewGuid(),
                    TeamName = "Home Team"
                };
                var team2 = new Team
                {
                    TeamId = Guid.NewGuid(),
                    TeamName = "Away Team"
                };
                _context.Teams.Add(team1);
                _context.Teams.Add(team2);

                var game = new Game
                {
                    GameStartDate = DateTime.UtcNow.Date, 
                    GameStartTime = DateTime.UtcNow,      
                    HomeTeamId = team1.TeamId,
                    AwayTeamId = team2.TeamId,
                    TournamentId = tournament.TournamentId,
                    AreTeamsConfirmed = true
                };
                _context.Games.Add(game);
                _context.SaveChanges();


            // Act
            using var response = await _client.GetAsync("/Games/Details/" + game.GamesId);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_return_succsess_when_game_is_found()
        {
            // Arrange
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship"
            };
            _context.Tournaments.Add(tournament);

            var team1 = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Home Team"
            };
            var team2 = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Away Team"
            };
            _context.Teams.Add(team1);
            _context.Teams.Add(team2);

            var game = new Game
            {
                GamesId = Guid.NewGuid(),
                GameStartDate = DateTime.UtcNow.Date,
                GameStartTime = DateTime.UtcNow,
                HomeTeamId = team1.TeamId,
                AwayTeamId = team2.TeamId,
                TournamentId = tournament.TournamentId,
                AreTeamsConfirmed = true
            };
            _context.Games.Add(game);
            _context.SaveChanges();


            // Act
            using var response = await _client.GetAsync("/Games/Edit/" + game.GamesId);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_return_succsess_when_game_is_found()
        {
            // Arrange
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship"
            };
            _context.Tournaments.Add(tournament);

            var team1 = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Home Team"
            };
            var team2 = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Away Team"
            };
            _context.Teams.Add(team1);
            _context.Teams.Add(team2);

            var game = new Game
            {
                GamesId = Guid.NewGuid(),
                GameStartDate = DateTime.UtcNow.Date,
                GameStartTime = DateTime.UtcNow,
                HomeTeamId = team1.TeamId,
                AwayTeamId = team2.TeamId,
                TournamentId = tournament.TournamentId,
                AreTeamsConfirmed = true
            };
            _context.Games.Add(game);
            _context.SaveChanges();


            // Act
            using var response = await _client.GetAsync("/Games/Delete/" + game.GamesId);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        //POST

        [Fact]
        public async Task Create_should_save_new_game()
        {
            // Arrange
            
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

            //Create teams
            var team1FormValues = new Dictionary<string, string>
            {
                { "TeamName", "Team A" },
                { "GoalsScored", "10" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "3" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            var team2FormValues = new Dictionary<string, string>
            {
                { "TeamName", "Team B" },
                { "GoalsScored", "1" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "1" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            using var team1Content = new FormUrlEncodedContent(team1FormValues);
            using var team2Content = new FormUrlEncodedContent(team2FormValues);

            using var team1Response = await _client.PostAsync("/Teams/Create", team1Content);
            using var team2Response = await _client.PostAsync("/Teams/Create", team2Content);

            var team1 = _context.Teams.FirstOrDefault(t => t.TeamName == "Team A");
            var team2 = _context.Teams.FirstOrDefault(t => t.TeamName == "Team B");

            Assert.NotNull(team1);
            Assert.NotNull(team2);


            //Create a new game
            var gameFormValues = new Dictionary<string, string>
            {
                { "GameStartDate", DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd") },
                { "GameStartTime", DateTime.UtcNow.AddHours(3).ToString("HH:mm") },
                { "AreTeamsConfirmed", "false" },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "HomeTeamId", team1.TeamId.ToString() },
                { "AwayTeamId", team2.TeamId.ToString() }
            };

            using var gameContent = new FormUrlEncodedContent(gameFormValues);

            // Act - Create the game
            using var gameResponse = await _client.PostAsync("/Games/Create", gameContent);

            // Assert - Check redirection
            Assert.True(gameResponse.StatusCode == HttpStatusCode.Redirect || gameResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Check if the game was created
            var game = _context.Games.FirstOrDefault(g => g.TournamentId == tournament.TournamentId);
            Assert.NotNull(game);
            Assert.NotEqual(Guid.Empty, game.GamesId);
            Assert.Equal(tournament.TournamentId, game.TournamentId);
            Assert.Equal(team1.TeamId, game.HomeTeamId);
        }

        [Fact]
        public async Task Create_should_not_save_new_game_with_incorrect_data()
        {
            // Arrange - Create tournament
            var tournament = new Tournament { TournamentName = "Champions Cup", Status = TournamentStatus.NotStarted, Format = TournamentFormat.EightTeams };
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            // Act 
                    var gameFormValues = new Dictionary<string, string>
            {
                { "GameStartDate", "" }, // Invalid date
                { "GameStartTime", "invalid-time" }, // Invalid time format
                { "AreTeamsConfirmed", "false" },
                { "TournamentId", tournament.TournamentId.ToString() }
            };

            using var gameContent = new FormUrlEncodedContent(gameFormValues);
            using var gameResponse = await _client.PostAsync("/Games/Create", gameContent);

            // Assert - Ensure game is not created
            gameResponse.EnsureSuccessStatusCode();
            Assert.False(_context.Users.Any());
        }

        [Fact]
        public async Task Edit_should_update_game()
        {
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

            //Create teams
            var team1FormValues = new Dictionary<string, string>
            {
                { "TeamName", "Team A" },
                { "GoalsScored", "10" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "3" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            var team2FormValues = new Dictionary<string, string>
            {
                { "TeamName", "Team B" },
                { "GoalsScored", "1" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "1" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            using var team1Content = new FormUrlEncodedContent(team1FormValues);
            using var team2Content = new FormUrlEncodedContent(team2FormValues);

            using var team1Response = await _client.PostAsync("/Teams/Create", team1Content);
            using var team2Response = await _client.PostAsync("/Teams/Create", team2Content);

            var team1 = _context.Teams.FirstOrDefault(t => t.TeamName == "Team A");
            var team2 = _context.Teams.FirstOrDefault(t => t.TeamName == "Team B");

            Assert.NotNull(team1);
            Assert.NotNull(team2);


            //Create a new game
            var gameFormValues = new Dictionary<string, string>
            {
                { "GameStartDate", DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd") },
                { "GameStartTime", DateTime.UtcNow.AddHours(3).ToString("HH:mm") },
                { "AreTeamsConfirmed", "true" },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "HomeTeamId", team1.TeamId.ToString() },
                { "AwayTeamId", team2.TeamId.ToString() }
            };

            using var gameContent = new FormUrlEncodedContent(gameFormValues);

            using var gameResponse = await _client.PostAsync("/Games/Create", gameContent);
            var game = _context.Games.FirstOrDefault(t => t.AreTeamsConfirmed == true);
            Assert.NotNull(game);

            // Detach game to prevent tracking issues
            _context.Entry(game).State = EntityState.Detached;

            // Prepare edit form data
            var editFormValues = new Dictionary<string, string>
            {
                { "GamesId", game.GamesId.ToString() },
                { "GameStartDate", DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd") },
                { "GameStartTime", DateTime.UtcNow.AddHours(5).ToString("HH:mm") },
                { "AreTeamsConfirmed", "true" },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "HomeTeamId", team1.TeamId.ToString() },
                { "AwayTeamId", team2.TeamId.ToString() }
            };

            //Act
            using var editContent = new FormUrlEncodedContent(editFormValues);
            using var editResponse = await _client.PostAsync($"/Games/Edit/{game.GamesId}", editContent);

            // Assert
            Assert.True(editResponse.StatusCode == HttpStatusCode.Redirect || editResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Verify the update
            var updatedGame = _context.Games.FirstOrDefault(g => g.GamesId == game.GamesId);
            Assert.NotNull(updatedGame);
            Assert.Equal(DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd"), updatedGame.GameStartDate.ToString("yyyy-MM-dd"));
            Assert.Equal(DateTime.UtcNow.AddHours(5).ToString("HH:mm"), updatedGame.GameStartTime.ToString("HH:mm"));
            Assert.True(updatedGame.AreTeamsConfirmed);
        }
        [Fact]
        public async Task Edit_should_not_update_game_with_incorrect_data()
        {
            // Arrange
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

            //Create teams
            var team1FormValues = new Dictionary<string, string>
            {
                { "TeamName", "Team A" },
                { "GoalsScored", "10" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "3" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            var team2FormValues = new Dictionary<string, string>
            {
                { "TeamName", "Team B" },
                { "GoalsScored", "1" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "1" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            using var team1Content = new FormUrlEncodedContent(team1FormValues);
            using var team2Content = new FormUrlEncodedContent(team2FormValues);

            using var team1Response = await _client.PostAsync("/Teams/Create", team1Content);
            using var team2Response = await _client.PostAsync("/Teams/Create", team2Content);

            var team1 = _context.Teams.FirstOrDefault(t => t.TeamName == "Team A");
            var team2 = _context.Teams.FirstOrDefault(t => t.TeamName == "Team B");

            Assert.NotNull(team1);
            Assert.NotNull(team2);


            //Create a new game
            var gameFormValues = new Dictionary<string, string>
            {
                { "GameStartDate", DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd") },
                { "GameStartTime", DateTime.UtcNow.AddHours(3).ToString("HH:mm") },
                { "AreTeamsConfirmed", "true" },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "HomeTeamId", team1.TeamId.ToString() },
                { "AwayTeamId", team2.TeamId.ToString() }
            };

            using var gameContent = new FormUrlEncodedContent(gameFormValues);

            using var gameResponse = await _client.PostAsync("/Games/Create", gameContent);
            var game = _context.Games.FirstOrDefault(t => t.AreTeamsConfirmed == true);
            Assert.NotNull(game);

            // Detach game to prevent tracking issues
            _context.Entry(game).State = EntityState.Detached;

            // Prepare edit form data
            var editFormValues = new Dictionary<string, string>
            {
                { "GamesId", game.GamesId.ToString() },
                { "GameStartDate","wrong" },
                { "GameStartTime", "lol" },
                { "AreTeamsConfirmed", "false" },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "HomeTeamId", team1.TeamId.ToString() },
                { "AwayTeamId","" }
            };

            using var editContent = new FormUrlEncodedContent(editFormValues);
            using var editResponse = await _client.PostAsync($"/Games/Edit/{game.GamesId}", editContent);
            //Assert
            editResponse.EnsureSuccessStatusCode();
            var updatedGame = _context.Games.FirstOrDefault(g => g.GamesId == game.GamesId);
            Assert.NotNull(updatedGame);
            Assert.True(updatedGame.AreTeamsConfirmed);

        }
        [Fact]
        public async Task Delete_should_delete_game()
        {
            // Arrange
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

            //Create teams
            var team1FormValues = new Dictionary<string, string>
            {
                { "TeamName", "Team A" },
                { "GoalsScored", "10" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "3" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            var team2FormValues = new Dictionary<string, string>
            {
                { "TeamName", "Team B" },
                { "GoalsScored", "1" },
                { "GoalsScoredAgainstThem", "5" },
                { "GamesWon", "1" },
                { "GamesLost", "2" },
                { "GamesPlayed", "5" }
            };

            using var team1Content = new FormUrlEncodedContent(team1FormValues);
            using var team2Content = new FormUrlEncodedContent(team2FormValues);

            using var team1Response = await _client.PostAsync("/Teams/Create", team1Content);
            using var team2Response = await _client.PostAsync("/Teams/Create", team2Content);

            var team1 = _context.Teams.FirstOrDefault(t => t.TeamName == "Team A");
            var team2 = _context.Teams.FirstOrDefault(t => t.TeamName == "Team B");

            Assert.NotNull(team1);
            Assert.NotNull(team2);


            //Create a new game
            var gameFormValues = new Dictionary<string, string>
            {
                { "GameStartDate", DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd") },
                { "GameStartTime", DateTime.UtcNow.AddHours(3).ToString("HH:mm") },
                { "AreTeamsConfirmed", "true" },
                { "TournamentId", tournament.TournamentId.ToString() },
                { "HomeTeamId", team1.TeamId.ToString() },
                { "AwayTeamId", team2.TeamId.ToString() }
            };

            using var gameContent = new FormUrlEncodedContent(gameFormValues);

            using var gameResponse = await _client.PostAsync("/Games/Create", gameContent);
            var game = _context.Games.FirstOrDefault(t => t.AreTeamsConfirmed == true);
            Assert.NotNull(game);

            // Detach game to prevent tracking issues
            _context.Entry(game).State = EntityState.Detached;

            // Act - Delete the team
            using var deleteResponse = await _client.PostAsync($"/Games/Delete/{game.GamesId}", null);

            // Assert
            // Check for redirect after successful deletion
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.Redirect || deleteResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Ensure the game was deleted from DB
            var deletedGame = _context.Games.FirstOrDefault(g => g.GamesId == game.GamesId);
            Assert.Null(deletedGame);
        }
    }
}
