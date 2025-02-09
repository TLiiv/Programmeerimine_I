using KooliProjekt.Data;
using KooliProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class GamesServiceTests : ServiceTestBase
    {
        private readonly GamesService _service;
        public GamesServiceTests()
        {
            _service = new GamesService(DbContext);
        }

        [Fact]
        public async Task AllGames_Should_Get_All_Games_And_Relevant_Data()
        {
            //Arrange
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "World Cup"
            };

            var homeTeam = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team A"
            };

            var awayTeam = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team B"
            };


            var game1 = new Game
            {
                GamesId = Guid.NewGuid(),
                HomeTeamId = homeTeam.TeamId,
                AwayTeamId = awayTeam.TeamId,
                TournamentId = tournament.TournamentId,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Tournament = tournament
            };

            var game2 = new Game
            {
                GamesId = Guid.NewGuid(),
                HomeTeamId = homeTeam.TeamId,
                AwayTeamId = awayTeam.TeamId,
                TournamentId = tournament.TournamentId,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Tournament = tournament
            };

            await DbContext.AddAsync(tournament);
            await DbContext.AddAsync(homeTeam);
            await DbContext.AddAsync(awayTeam);
            await DbContext.AddAsync(game1);
            await DbContext.AddAsync(game2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.AllGames();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var firstGame = result.First();
            Assert.NotNull(firstGame.HomeTeam);
        }
        [Fact]
        public async Task Get_Should_Fetch_Game_By_Id_And_Relevant_Data()
        {
            // Arrange
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "World Cup"
            };

            var homeTeam = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team A"
            };

            var awayTeam = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team B"
            };

            var game = new Game
            {
                GamesId = Guid.NewGuid(),  
                HomeTeamId = homeTeam.TeamId,
                AwayTeamId = awayTeam.TeamId,
                TournamentId = tournament.TournamentId,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Tournament = tournament
            };

            // Add entities to the DbContext
            await DbContext.AddAsync(tournament);
            await DbContext.AddAsync(homeTeam);
            await DbContext.AddAsync(awayTeam);
            await DbContext.AddAsync(game);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(game.GamesId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(game.GamesId, result.GamesId);

            Assert.NotNull(result.HomeTeam);
            Assert.Equal("Team A", result.HomeTeam.TeamName);
        }
        [Fact]
        public async Task GetDropdownData_Should_Return_Teams_And_Tournaments()
        {
            // Arrange
            var team1 = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team A"
            };

            var team2 = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team B"
            };

            var tournament1 = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "World Cup"
            };

            var tournament2 = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Euro Cup"
            };

            // Add entities to the DbContext
            await DbContext.AddAsync(team1);
            await DbContext.AddAsync(team2);
            await DbContext.AddAsync(tournament1);
            await DbContext.AddAsync(tournament2);
            await DbContext.SaveChangesAsync();

            // Act
            var (teams, tournaments) = await _service.GetDropdownData();

            // Assert
            Assert.NotNull(teams);  
            Assert.NotNull(tournaments);  

            Assert.Equal(2, teams.Count);
            Assert.Equal(2, tournaments.Count);
        }
        [Fact]
        public async Task Save_Should_Save_UserBet_And_Add_Id()
        {
            // Arrange
            var game = new Game
            {
                GamesId = Guid.Empty,  
                HomeTeam = new Team { TeamId = Guid.NewGuid(), TeamName = "Home Team" },
                AwayTeam = new Team { TeamId = Guid.NewGuid(), TeamName = "Away Team" },
                Tournament = new Tournament { TournamentId = Guid.NewGuid(), TournamentName = "World Cup" },
            };

            // Act
            await _service.Save(game);

            // Assert
            Assert.NotEqual(Guid.Empty, game.GamesId); 
            var savedGame = await DbContext.Games.FindAsync(game.GamesId); 
            Assert.NotNull(savedGame); 
            Assert.Equal(game.GamesId, savedGame.GamesId); 
        }
        [Fact]
        public async Task Save_Should_Update_Existing_Game_When_GameId_Is_Provided()
        {
            var existingGame = new Game
            {
                GamesId = Guid.NewGuid(),
                HomeTeam = new Team { TeamId = Guid.NewGuid(), TeamName = "Home Team" },
                AwayTeam = new Team { TeamId = Guid.NewGuid(), TeamName = "Away Team" },
                Tournament = new Tournament { TournamentId = Guid.NewGuid(), TournamentName = "Euro Cup" },
            };

            await DbContext.AddAsync(existingGame);
            await DbContext.SaveChangesAsync();  

            // Act
            existingGame.Tournament.TournamentName = "Champions League";
            await _service.Save(existingGame);

            // Assert
            var updatedGame = await DbContext.Games.FindAsync(existingGame.GamesId);
            Assert.NotNull(updatedGame);
            Assert.Equal("Champions League", updatedGame.Tournament.TournamentName); 
        }
    }
}
    
