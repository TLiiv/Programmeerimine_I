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
    public class TournamentsServiceTests : ServiceTestBase
    {
        private readonly TournamentsService _service;
        public TournamentsServiceTests()
        {
            _service = new TournamentsService(DbContext);
        }
        [Fact]
        public async Task AllTournaments_Should_Get_All_Tournaments()
        {
            // Arrange
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

            await DbContext.AddAsync(tournament1);
            await DbContext.AddAsync(tournament2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.AllTournaments();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

        }
        [Fact]
        public async Task Get_Should_Fetch_Tournament_By_Id()
        {
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Champions League"
            };

            await DbContext.AddAsync(tournament);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(tournament.TournamentId);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Equal(tournament.TournamentId, result.TournamentId); // Ensure the fetched tournament matches the id
            Assert.Equal("Champions League", result.TournamentName);
        }
        [Fact]
        public async Task Save_Should_Add_Tournament_And_Assign_Id_When_Id_Is_Empty()
        {
            // Arrange
            var tournament = new Tournament
            {
                TournamentName = "Premier League"
            };

            // Act
            await _service.Save(tournament);

            // Assert
            Assert.NotEqual(Guid.Empty, tournament.TournamentId);
            Assert.NotNull(await DbContext.Tournaments.FindAsync(tournament.TournamentId));
            Assert.Equal("Premier League", tournament.TournamentName);
        }
        [Fact]
        public async Task Save_Should_Update_Tournament_When_Id_Is_Provided()
        {
            // Arrange
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Champions League"
            };

            await DbContext.AddAsync(tournament);
            await DbContext.SaveChangesAsync();

            // Act
            tournament.TournamentName = "Updated Champions League"; 
            await _service.Save(tournament); 

            // Assert
            var updatedTournament = await DbContext.Tournaments.FindAsync(tournament.TournamentId);
            Assert.NotNull(updatedTournament); 
            Assert.Equal("Updated Champions League", updatedTournament.TournamentName);
        }
    }
}