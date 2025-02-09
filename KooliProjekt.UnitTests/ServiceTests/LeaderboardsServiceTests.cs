using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class LeaderboardsServiceTests : ServiceTestBase
    {
        private readonly LeaderboardsService _service;
        public LeaderboardsServiceTests()
        {
            _service = new LeaderboardsService(DbContext);
        }
        [Fact]
        public async Task AllLeaderboards_Should_Display_All_Leaderboards()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "TestUser",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password"
            };

            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship 2025"
            };

            var leaderboard1 = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                User = user,
                Tournament = tournament,

            };

            var leaderboard2 = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                User = user,
                Tournament = tournament,

            };

            await DbContext.AddAsync(user);
            await DbContext.AddAsync(tournament);
            await DbContext.AddAsync(leaderboard1);
            await DbContext.AddAsync(leaderboard2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.AllLeaderboards();

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Equal(2, result.Count); // Ensure two leaderboard entries are returned
            Assert.Equal("TestUser", result.First().User.UserName);
        }
        [Fact]
        public async Task Get_Should_Fetch_Leaderboard_By_Id()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "TestUser1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password",
            };

            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship 2025"
            };

            var leaderboard = new Leaderboard
            {
                LeaderBoardId = Guid.NewGuid(),
                User = user,
                Tournament = tournament,

            };

            await DbContext.AddAsync(user);
            await DbContext.AddAsync(tournament);
            await DbContext.AddAsync(leaderboard);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(leaderboard.LeaderBoardId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(leaderboard.LeaderBoardId, result.LeaderBoardId);
            Assert.Equal("TestUser1", result.User.UserName);
            Assert.Equal("Championship 2025", result.Tournament.TournamentName);
        }
        [Fact]
        public async Task GetDropdownData_Should_Return_Tournaments_And_Users()
        {
            // Arrange
            var tournament1 = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Tournament 1"
            };

            var tournament2 = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Tournament 2"
            };

            var user1 = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "User1",
                FirstName = "John",
                LastName = "Doe",
                Email = "user1@example.com",
                Password = "password"
            };

            var user2 = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "User2",
                FirstName = "Jane",
                LastName = "Doe",
                Email = "user2@example.com",
                Password = "password"
            };

            await DbContext.AddAsync(tournament1);
            await DbContext.AddAsync(tournament2);
            await DbContext.AddAsync(user1);
            await DbContext.AddAsync(user2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetDropdownData();

            // Assert

            Assert.Equal(2, result.Tournaments.Count); // Ensure two tournaments are returned
            Assert.Contains(result.Tournaments, t => t.TournamentName == "Tournament 2"); // Check if Tournament 2 is in the result
            Assert.Equal(2, result.Users.Count); // Ensure two users are returned
            Assert.Contains(result.Users, u => u.UserName == "User2"); // Check if User2 is in the result
        }
        [Fact]
        public async Task Save_Should_Add_Leaderboard_And_Assign_Id_When_Id_Is_Empty()
        {
            // Arrange
            var leaderboard = new Leaderboard
            {
                LeaderBoardId = Guid.Empty,
                UserId = Guid.NewGuid(),
                TournamentId = Guid.NewGuid(),
            };

            // Act
            await _service.Save(leaderboard);

            // Assert
            Assert.NotEqual(Guid.Empty, leaderboard.LeaderBoardId);
            var savedLeaderboard = await DbContext.Leaderboards.FindAsync(leaderboard.LeaderBoardId);
            Assert.NotNull(savedLeaderboard);
        }
        [Fact]
        public async Task Save_Should_Update_Leaderboard_When_Id_Is_Provided()
        {
            // Arrange
            var leaderboardId = Guid.NewGuid();
            var existingLeaderboard = new Leaderboard
            {
                LeaderBoardId = leaderboardId,
                UserId = Guid.NewGuid(),
                TournamentId = Guid.NewGuid(),
                Rank = 1
            };

            await DbContext.Leaderboards.AddAsync(existingLeaderboard);
            await DbContext.SaveChangesAsync();

            // Modify the leaderboard data for update
            existingLeaderboard.Rank = 2;

            // Act
            await _service.Save(existingLeaderboard);

            // Assert
            var updatedLeaderboard = await DbContext.Leaderboards.FindAsync(leaderboardId);
            Assert.NotNull(updatedLeaderboard);
            Assert.Equal(2, updatedLeaderboard.Rank);
        }
    }
}
