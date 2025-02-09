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
    public class UserBetsServiceTests : ServiceTestBase
    {
        private readonly UserBetsService _service;
        public UserBetsServiceTests()
        {
            _service = new UserBetsService(DbContext);
        }
        [Fact]
        public async Task AllUserBets_Should_Display_All_Bets()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "TestUser1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                UserBets = new List<UserBets>
                {
                        new UserBets
                        {
                        Id = Guid.NewGuid(),
                        UserId = Guid.NewGuid(), // Link to the user
                        PredictedHomeGoals = 2,
                        PredictedAwayGoals = 1,
                        AccountBalance = 500.0,
                        BetAmount = 50.0,
                        BetPlacedDate = DateTime.UtcNow,
                        GameId = Guid.NewGuid(), // Link to the game
                        Game = new Game
                        {
                            GamesId = Guid.NewGuid(), // Game ID
                            
                        },
                        PredictedWinningTeamId = Guid.NewGuid(),
                        PredictedWinningTeam = new Team
                        {
                            TeamId = Guid.NewGuid(),
                            TeamName ="Random"

                        },
                        TournamentId = Guid.NewGuid(),
                        Tournament = new Tournament
                        {
                            TournamentId = Guid.NewGuid(),
                            TournamentName = "Humangus Cup"

                        }
                    }
                }
            };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            //Act
            var result = await _service.AllUserBets();

            //Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
        [Fact]
        public async Task Get_Should_Fetch_UserBet_By_Id()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "TestUser1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                UserBets = new List<UserBets>
                {
                new UserBets
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    PredictedHomeGoals = 2,
                    PredictedAwayGoals = 1,
                    AccountBalance = 500.0,
                    BetAmount = 50.0,
                    BetPlacedDate = DateTime.UtcNow,
                    GameId = Guid.NewGuid(),
                    Game = new Game
                    {
                        GamesId = Guid.NewGuid(),
                    },
                    PredictedWinningTeamId = Guid.NewGuid(),
                    PredictedWinningTeam = new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "Random"
                    },
                    TournamentId = Guid.NewGuid(),
                    Tournament = new Tournament
                    {
                        TournamentId = Guid.NewGuid(),
                        TournamentName = "Humangus Cup"
                    }
                }
            }
        };

            // Add user and their bets to the database
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(user.UserBets.First().Id);

            // Assert
            Assert.NotNull(result);  // Ensure the result is not null
            Assert.Equal(user.UserBets.First().Id, result.Id);
        }

        [Fact]
        public async Task GetDropdownData_Should_Return_Expected_Data()
        {
            // Arrange
            var game1 = new Game
            {
                GamesId = Guid.NewGuid(),
            };
            var game2 = new Game
            {
                GamesId = Guid.NewGuid(),
            };

            var team1 = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team1",
            };
            var team2 = new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Team2",
            };

            var tournament1 = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Tournament1",
            };
            var tournament2 = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Tournament2",
            };
            await DbContext.AddAsync(game1);
            await DbContext.AddAsync(game2);
            await DbContext.AddAsync(team1);
            await DbContext.AddAsync(team2);
            await DbContext.AddAsync(tournament1);
            await DbContext.AddAsync(tournament2);
            await DbContext.SaveChangesAsync();
            // Act
            var result = await _service.GetDropdownData();

            //Assert
            Assert.Equal(2, result.Games.Count);
            Assert.Equal(2, result.Teams.Count);
            Assert.Equal(2, result.Tournaments.Count);
        }
        [Fact]
        public async Task Save_Should_Save_UserBet_And_Add_Id()
        {
            //Arrange
            var userbet = new UserBets
            {
                UserId = Guid.NewGuid(),
                PredictedHomeGoals = 2,
                PredictedAwayGoals = 1,
                AccountBalance = 500.0,
                BetAmount = 50.0,
                BetPlacedDate = DateTime.UtcNow,
                GameId = Guid.NewGuid(),
                Game = new Game
                {
                    GamesId = Guid.NewGuid(),
                },
                PredictedWinningTeamId = Guid.NewGuid(),
                PredictedWinningTeam = new Team
                {
                    TeamId = Guid.NewGuid(),
                    TeamName = "Random"
                },
                TournamentId = Guid.NewGuid(),
                Tournament = new Tournament
                {
                    TournamentId = Guid.NewGuid(),
                    TournamentName = "Humangus Cup"
                }
            };

            //Act
            await _service.Save(userbet);

            //Assert

            // Verify that the UserBets record was added
            var savedUserBet = await DbContext.UsersBets
                .FirstOrDefaultAsync(ub => ub.UserId == userbet.UserId && ub.GameId == userbet.GameId);

            Assert.NotNull(savedUserBet);
            Assert.NotNull(savedUserBet.Id);

        }

        [Fact]
        public async Task Save_Should_Update_Existing_User_When_User_Bet_Id_Is_Provided()
        {
            //Arrange
            var existingUserbet = new UserBets
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PredictedHomeGoals = 2,
                PredictedAwayGoals = 1,
                AccountBalance = 500.0,
                BetAmount = 50.0,
                BetPlacedDate = DateTime.UtcNow,
                GameId = Guid.NewGuid(),
                Game = new Game
                {
                    GamesId = Guid.NewGuid(),
                },
                PredictedWinningTeamId = Guid.NewGuid(),
                PredictedWinningTeam = new Team
                {
                    TeamId = Guid.NewGuid(),
                    TeamName = "Random"
                },
                TournamentId = Guid.NewGuid(),
                Tournament = new Tournament
                {
                    TournamentId = Guid.NewGuid(),
                    TournamentName = "Humangus Cup"
                }
            };

            await DbContext.AddAsync(existingUserbet);
            await DbContext.SaveChangesAsync();

            //Act
            existingUserbet.PredictedHomeGoals = 10;
            await _service.Save(existingUserbet);

            //Assert

            // Verify that the UserBets record was added
            var updatedUserbet = await DbContext.UsersBets.FirstOrDefaultAsync(u => u.Id == existingUserbet.Id);

            Assert.NotNull(updatedUserbet);
            Assert.Equal(existingUserbet.PredictedHomeGoals, updatedUserbet.PredictedHomeGoals);

        }
    }
}
