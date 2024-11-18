using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {


            if (!context.Users.Any())
            {

                var users = new List<User>
                {
                    new User
                    {
                        UserName = "AwesomeUser",
                        FirstName = "Jaan",
                        LastName = "Mustikas",
                        Email = "jaan@mustikas.ee",
                        Password = "password",
                        PhoneNumber = "021987309",
                        IsAdmin = false
                    },
                    new User
                    {
                        UserName = "CoolUser",
                        FirstName = "Jaanus",
                        LastName = "Kaalikas",
                        Email = "jaanus@kaalikas.ee",
                        Password = "password",
                        PhoneNumber = "021987309",
                        IsAdmin = false
                    },
                    new User
                    {
                        UserName = "LameUser",
                        FirstName = "Kristjan",
                        LastName = "Kuul",
                        Email = "kristjan@kuul.ee",
                        Password = "password",
                        PhoneNumber = "021987309",
                        IsAdmin = false
                    },
                    new User
                    {
                        UserName = "CheatingUser",
                        FirstName = "Jaana",
                        LastName = "Vaarikas",
                        Email = "jaana@vaarikas.ee",
                        Password = "password",
                        PhoneNumber = "021987309",
                        IsAdmin = false
                    }
                };
                context.AddRange(users);
                context.SaveChanges();

                if (!context.Teams.Any())
                {
                    var teams = new List<Team>
                {
                    new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "Manchester United",
                        GoalsScored = 85,
                        GoalsScoredAgainstThem = 45,
                        GamesWon = 27,
                        GamesLost = 5,
                        GamesPlayed = 38
                    },
                    new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "Liverpool FC",
                        GoalsScored = 90,
                        GoalsScoredAgainstThem = 38,
                        GamesWon = 30,
                        GamesLost = 4,
                        GamesPlayed = 38
                    },
                    new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "Chelsea FC",
                        GoalsScored = 65,
                        GoalsScoredAgainstThem = 40,
                        GamesWon = 19,
                        GamesLost = 9,
                        GamesPlayed = 38
                    },
                    new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "Arsenal FC",
                        GoalsScored = 70,
                        GoalsScoredAgainstThem = 43,
                        GamesWon = 22,
                        GamesLost = 8,
                        GamesPlayed = 38
                    },
                    new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "Manchester City",
                        GoalsScored = 95,
                        GoalsScoredAgainstThem = 30,
                        GamesWon = 32,
                        GamesLost = 3,
                        GamesPlayed = 38
                    },
                    new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "Tottenham Hotspur",
                        GoalsScored = 62,
                        GoalsScoredAgainstThem = 50,
                        GamesWon = 18,
                        GamesLost = 12,
                        GamesPlayed = 38
                    },
                    new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "Leicester City",
                        GoalsScored = 60,
                        GoalsScoredAgainstThem = 50,
                        GamesWon = 17,
                        GamesLost = 12,
                        GamesPlayed = 38
                    },
                    new Team
                    {
                        TeamId = Guid.NewGuid(),
                        TeamName = "West Ham United",
                        GoalsScored = 58,
                        GoalsScoredAgainstThem = 55,
                        GamesWon = 16,
                        GamesLost = 14,
                        GamesPlayed = 38
                    }
                };
                    context.AddRange(teams);
                    context.SaveChanges();
                }

                if (!context.Tournaments.Any())
                {
                    var teams = context.Teams.ToList();

                    var tournaments = new List<Tournament>
                {
                    new Tournament
                    {
                        TournamentName = "Awesome Cup",
                        TournamentStartDate = new DateTime(2024,12,1),
                        TournamentEndtDate = new DateTime(2025,1,1),
                        Status =TournamentStatus.NotStarted,
                        Format = TournamentFormat.EightTeams,
         
            }
        
    };

                    context.AddRange(tournaments);
                    context.SaveChanges();
                }
            }
        }
    }
}
