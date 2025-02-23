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
    public class GamesControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public GamesControllerTests()
        {
            _client = Factory.CreateClient();
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
    }
}
