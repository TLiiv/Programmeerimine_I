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
    public class UserBetsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public UserBetsControllerTests()
        {
            _client = Factory.CreateClient();
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));

        }
        //GET METHODS
        [Theory]
        [InlineData("/UserBets")]
        [InlineData("/UserBets/Create")]
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
        [InlineData("/UserBets/Details/")]
        [InlineData("/UserBets/Edit/")]
        [InlineData("/UserBets/Delete/")]
        [InlineData("/UserBets/Details/1")]
        [InlineData("/UserBets/Edit/1")]
        [InlineData("/UserBets/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_bet_id_is_missing_or_not_found(string url)
        {
            //Arrange
            
            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

       

        [Fact]
        public async Task Details_should_return_succsess_when_user_bet_is_found()
        {
            //Arrange
            var user = new User { UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com", Password = "asd", PhoneNumber = "51231231", IsAdmin = true };
            _context.Users.Add(user);

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
                HomeTeamId = team1.TeamId,
                AwayTeamId = team2.TeamId,
                TournamentId = tournament.TournamentId
            };
            _context.Games.Add(game);

           
            _context.SaveChanges();

            var userBet = new UserBets
            {
                //Id = Guid.NewGuid(),
                UserId = user.UserId,
                TournamentId = tournament.TournamentId, 
                GameId = game.GamesId,  
                PredictedWinningTeamId = team1.TeamId, 
                PredictedHomeGoals = 2,
                PredictedAwayGoals = 1,
                AccountBalance = 100.0,
                BetAmount = 10.0
            };
            _context.UsersBets.Add(userBet);
            _context.SaveChanges(); 

            //Act
            using var response = await _client.GetAsync("/UserBets/Details/" + userBet.Id);
            //Assert
            response.EnsureSuccessStatusCode();

        }
        [Fact]
        public async Task Edit_should_return_succsess_when_user_bet_is_found()
        {
            //Arrange
            var user = new User { UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com", Password = "asd", PhoneNumber = "51231231", IsAdmin = true };
            _context.Users.Add(user);

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
                HomeTeamId = team1.TeamId,
                AwayTeamId = team2.TeamId,
                TournamentId = tournament.TournamentId
            };
            _context.Games.Add(game);


            _context.SaveChanges();

            var userBet = new UserBets
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                TournamentId = tournament.TournamentId,
                GameId = game.GamesId,
                PredictedWinningTeamId = team1.TeamId,
                PredictedHomeGoals = 2,
                PredictedAwayGoals = 1,
                AccountBalance = 100.0,
                BetAmount = 10.0
            };
            _context.UsersBets.Add(userBet);
            _context.SaveChanges();

            //Act
            using var response = await _client.GetAsync("/UserBets/Edit/" + userBet.Id);
            //Assert
            response.EnsureSuccessStatusCode();

        }
        [Fact]
        public async Task Delete_should_return_succsess_when_user_bet_is_found()
        {
            //Arrange
            var user = new User { UserName = "user1", FirstName = "First", LastName = "User", Email = "user1@example.com", Password = "asd", PhoneNumber = "51231231", IsAdmin = true };
            _context.Users.Add(user);

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
                HomeTeamId = team1.TeamId,
                AwayTeamId = team2.TeamId,
                TournamentId = tournament.TournamentId
            };
            _context.Games.Add(game);


            _context.SaveChanges();

            var userBet = new UserBets
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                TournamentId = tournament.TournamentId,
                GameId = game.GamesId,
                PredictedWinningTeamId = team1.TeamId,
                PredictedHomeGoals = 2,
                PredictedAwayGoals = 1,
                AccountBalance = 100.0,
                BetAmount = 10.0
            };
            _context.UsersBets.Add(userBet);
            _context.SaveChanges();

            //Act
            using var response = await _client.GetAsync("/UserBets/Delete/" + userBet.Id);
            //Assert
            response.EnsureSuccessStatusCode();

        }
    }
}
