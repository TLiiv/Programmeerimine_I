using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class TeamsServiceTests : ServiceTestBase
    {
        private readonly TeamsService _service;
        public TeamsServiceTests()
        {
            _service = new TeamsService(DbContext);
        }
        [Fact]
        public async Task AllTeams_Should_Display_All_Teams_Without_Keyword()
        {
            // Arrange
            var team1 = new Team { TeamName = "Team A" };
            var team2 = new Team { TeamName = "Team B" };

            await DbContext.AddAsync(team1);
            await DbContext.AddAsync(team2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.AllTeams();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.TeamName == "Team A");
            Assert.Contains(result, t => t.TeamName == "Team B");
        }

        [Fact]
        public async Task AllTeams_Should_Display_Teams_By_Keyword()
        {
            // Arrange
            var team1 = new Team { TeamName = "Team Alpha" };
            var team2 = new Team { TeamName = "Team Bravo" };
            var team3 = new Team { TeamName = "Team Charlie" };

            await DbContext.AddAsync(team1);
            await DbContext.AddAsync(team2);
            await DbContext.AddAsync(team3);
            await DbContext.SaveChangesAsync();

            var search = new TeamsSearch { Keyword = "Alpha" }; 

            // Act
            var result = await _service.AllTeams(search);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); 
            Assert.Contains(result, t => t.TeamName == "Team Alpha");
        }
        [Fact]
        public async Task Get_Should_Fetch_Team_By_Id()
        {
            // Arrange
            var team = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team Alpha"
            };

            await DbContext.AddAsync(team);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(team.TeamId);

            // Assert
            Assert.NotNull(result); 
            Assert.Equal(team.TeamId, result.TeamId); 
            Assert.Equal("Team Alpha", result.TeamName); 
        }
        [Fact]
        public async Task Save_Should_Add_Team_And_Assign_Id_When_Id_Is_Empty()
        {
            // Arrange
            var team = new Team
            {
                TeamId = Guid.Empty, 
                TeamName = "Team Bravo"
            };

            // Act
            await _service.Save(team);

            // Assert
            Assert.NotEqual(Guid.Empty, team.TeamId);
            var savedTeam = await DbContext.Teams.FindAsync(team.TeamId);
            Assert.NotNull(savedTeam); 
            Assert.Equal("Team Bravo", savedTeam.TeamName);
        }
        [Fact]
        public async Task Save_Should_Update_Team_When_Id_Is_Provided()
        {
            // Arrange
            var existingTeam = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team Alpha"
            };

            await DbContext.AddAsync(existingTeam);
            await DbContext.SaveChangesAsync();
            
            existingTeam.TeamName = "Team Alpha Updated";

            // Act
            await _service.Save(existingTeam);

            // Assert
            var updatedTeam = await DbContext.Teams.FindAsync(existingTeam.TeamId);
            Assert.NotNull(updatedTeam); 
            Assert.Equal("Team Alpha Updated", updatedTeam.TeamName);
        }
    }
}
