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
    public class LeaderboardsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public LeaderboardsControllerTests()
        {
            _client = Factory.CreateClient();
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
        public async Task Get_endpoints_should_return_not_found_when_bet_id_is_missing(string url)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/Leaderboards/Details/1")]
        [InlineData("/Leaderboards/Edit/1")]
        [InlineData("/Leaderboards/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_bet_does_not_exist(string url)
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
    }
}
