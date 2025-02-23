using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.CodeAnalysis.Elfie.Model.Structures;
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
    public class UserBetsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public UserBetsControllerTests()
        {
            // Turn of Redirection trackin (for post mehtod status codes (after user save etc there is redirection))
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
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

        //POST
        [Fact]
        public async Task Create_should_save_new_user_bet()
        {
            //Arrange

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

            //BET
            var betFormValues = new Dictionary<string, string>
             {
                 { "UserId", user.UserId.ToString() },
                 { "TournamentId", tournament.TournamentId.ToString() },
                 { "GameId", game.GamesId.ToString() },
                 { "PredictedWinningTeamId", game.AwayTeamId.ToString() },
                 { "PredictedHomeGoals", "2" },
                 { "PredictedAwayGoals", "1" },
                 { "AccountBalance", "500.0" },
                 { "BetAmount", "50.0" }
             };

            using var betContent = new FormUrlEncodedContent(betFormValues);


            // Act - Create the UserBet
            using var betResponse = await _client.PostAsync("/UserBets/Create", betContent);


            // Assert
            Assert.True(betResponse.StatusCode == HttpStatusCode.Redirect || betResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Retrieve the created UserBet
            var userBet = _context.UsersBets.FirstOrDefault();
            Assert.NotNull(userBet);
            Assert.Equal(user.UserId, userBet.UserId);
            Assert.Equal(tournament?.TournamentId, userBet.TournamentId);
            Assert.Equal(2, userBet.PredictedHomeGoals);
            Assert.Equal(1, userBet.PredictedAwayGoals);
            Assert.Equal(500.0, userBet.AccountBalance);
            Assert.Equal(50.0, userBet.BetAmount);
        }
        [Fact]
        public async Task Create_should_not_save_new_user_bet_when_invalid_data()
        {
            //Arrange

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

            //BET
            var betFormValues = new Dictionary<string, string>
             {
                 { "UserId", user.UserId.ToString() },
                 { "TournamentId", tournament.TournamentId.ToString() },
                 { "GameId", game.GamesId.ToString() },
                 { "PredictedWinningTeamId", "wrong data" },
                 { "PredictedHomeGoals", "2" },
                 { "PredictedAwayGoals", "1" },
                 { "AccountBalance", "500.0" },
                 { "BetAmount", "50.0" }
             };

            using var betContent = new FormUrlEncodedContent(betFormValues);


            // Act - Create the UserBet
            using var betResponse = await _client.PostAsync("/UserBets/Create", betContent);


            // Assert
            betResponse.EnsureSuccessStatusCode();
            Assert.False(_context.UsersBets.Any());
        }
        [Fact]
        public async Task Edit_should_update_user_bet()
        {
            //Arrange

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

            //Bet
            var betFormValues = new Dictionary<string, string>
             {
                 { "UserId", user.UserId.ToString() },
                 { "TournamentId", tournament.TournamentId.ToString() },
                 { "GameId", game.GamesId.ToString() },
                 { "PredictedWinningTeamId", game.AwayTeamId.ToString() },
                 { "PredictedHomeGoals", "2" },
                 { "PredictedAwayGoals", "1" },
                 { "AccountBalance", "500.0" },
                 { "BetAmount", "50.0" }
             };

            using var betContent = new FormUrlEncodedContent(betFormValues);
            using var betResponse = await _client.PostAsync("/UserBets/Edit", betContent);
            var userBet = _context.UsersBets.FirstOrDefault();
            Assert.NotNull(userBet);

            // Detach game to prevent tracking issues
            _context.Entry(userBet).State = EntityState.Detached;

            //Edit User Bet
            var editFormValues = new Dictionary<string, string>
             {
                 { "Id", userBet.Id.ToString()},
                 { "UserId", user.UserId.ToString() },
                 { "TournamentId", tournament.TournamentId.ToString() },
                 { "GameId", game.GamesId.ToString() },
                 { "PredictedWinningTeamId", game.AwayTeamId.ToString() },
                 { "PredictedHomeGoals", "4" },
                 { "PredictedAwayGoals", "0" },
                 { "AccountBalance", "500.0" },
                 { "BetAmount", "50.0" }
             };

            //Act
            using var editContent = new FormUrlEncodedContent(editFormValues);
            using var editResponse = await _client.PostAsync($"/UserBets/Edit/{userBet.Id}", editContent);

            // Assert
            editResponse.EnsureSuccessStatusCode();
            var updatedBet = _context.UsersBets.FirstOrDefault(b => b.Id == userBet.Id);
            Assert.NotNull(updatedBet);
            Assert.Equal(0 ,updatedBet.PredictedAwayGoals);
            Assert.Equal(4 ,updatedBet.PredictedHomeGoals);
            Assert.Equal(50.0, userBet.BetAmount);
        }
        [Fact]
        public async Task Edit_should_not_update_user_bet_when_incorrect_data()
        {
            //Arrange

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

            //BET
            var betFormValues = new Dictionary<string, string>
             {
                 { "UserId", user.UserId.ToString() },
                 { "TournamentId", tournament.TournamentId.ToString() },
                 { "GameId", game.GamesId.ToString() },
                 { "PredictedWinningTeamId", game.AwayTeamId.ToString() },
                 { "PredictedHomeGoals", "2" },
                 { "PredictedAwayGoals", "1" },
                 { "AccountBalance", "500.0" },
                 { "BetAmount", "50.0" }
             };

            using var betContent = new FormUrlEncodedContent(betFormValues);
            using var betResponse = await _client.PostAsync("/UserBets/Edit", betContent);
            var userBet = _context.UsersBets.FirstOrDefault();
            Assert.NotNull(userBet);

            // Detach game to prevent tracking issues
            _context.Entry(userBet).State = EntityState.Detached;

            //Edit User Bet
            var editFormValues = new Dictionary<string, string>
             {
                 { "Id", userBet.Id.ToString()},
                 { "UserId", user.UserId.ToString() },
                 { "TournamentId","wrong id" },
                 { "GameId", game.GamesId.ToString() },
                 { "PredictedWinningTeamId", game.AwayTeamId.ToString() },
                 { "PredictedHomeGoals", "4" },
                 { "PredictedAwayGoals", "0" },
                 { "AccountBalance", "500.0" },
                 { "BetAmount", "50.0" }
             };

            //Act
            using var editContent = new FormUrlEncodedContent(editFormValues);
            using var editResponse = await _client.PostAsync($"/UserBets/Edit/{userBet.Id}", editContent);

            //Assert
            
            editResponse.EnsureSuccessStatusCode();
            var updatedBet = _context.UsersBets.FirstOrDefault(b => b.Id == userBet.Id);
            Assert.NotNull(updatedBet);
            Assert.Equal(1, updatedBet.PredictedAwayGoals);
            Assert.Equal(2, updatedBet.PredictedHomeGoals);
            Assert.Equal(50.0, userBet.BetAmount);
        }
        [Fact]
        public async Task Delete_should_delete_user_bet()
        {
            //Arrange

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

            //BET
            var betFormValues = new Dictionary<string, string>
             {
                 { "UserId", user.UserId.ToString() },
                 { "TournamentId", tournament.TournamentId.ToString() },
                 { "GameId", game.GamesId.ToString() },
                 { "PredictedWinningTeamId", game.AwayTeamId.ToString() },
                 { "PredictedHomeGoals", "2" },
                 { "PredictedAwayGoals", "1" },
                 { "AccountBalance", "500.0" },
                 { "BetAmount", "50.0" }
             };

            using var betContent = new FormUrlEncodedContent(betFormValues);
            using var betResponse = await _client.PostAsync("/UserBets/Create", betContent);
            var userBet = _context.UsersBets.FirstOrDefault();
            // Detach user bet to prevent tracking issues
            _context.Entry(userBet).State = EntityState.Detached;


            // Act - Delete the team
            using var deleteResponse = await _client.PostAsync($"/UserBets/Delete/{userBet.Id}", null);



            // Assert
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.Redirect || deleteResponse.StatusCode == HttpStatusCode.MovedPermanently);
            var deletedUserBet = _context.UsersBets.FirstOrDefault();
            Assert.Null(deletedUserBet);

        }
    }


}
