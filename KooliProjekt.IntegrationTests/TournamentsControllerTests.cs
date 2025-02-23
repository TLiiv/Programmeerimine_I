using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
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
    public class TournamentsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public TournamentsControllerTests()
        {
            // Turn of Redirection trackin (for post mehtod status codes (after user save etc there is redirection))
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));

        }
        //GET METHODS
        [Theory]
        [InlineData("/Tournaments")]
        [InlineData("/Tournaments/Create")]
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
        [InlineData("/Tournaments/Details/")]
        [InlineData("/Tournaments/Edit/")]
        [InlineData("/Tournaments/Delete/")]
        [InlineData("/Tournaments/Details/1")]
        [InlineData("/Tournaments/Edit/1")]
        [InlineData("/Tournaments/Delete/1")]
        public async Task Get_endpoints_should_return_not_found_when_tournaments_id_is_missing_or_not_found(string url)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(url);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

       
        [Fact]
        public async Task Details_should_return_success_when_tournament_is_found()
        {
            // Arrange
            var tournament = new Tournament
            {
                TournamentName = "Championship",
                TournamentStartDate = DateTime.Now.AddMonths(-1),
                TournamentEndtDate = DateTime.Now.AddMonths(1),
                Status = TournamentStatus.Ongoing,
                Format = TournamentFormat.SixteenTeams
            };
            _context.Tournaments.Add(tournament);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Tournaments/Details/"+ tournament.TournamentId);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Edit_should_return_success_when_tournament_is_found()
        {
            // Arrange
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship",
                TournamentStartDate = DateTime.Now.AddMonths(-1),
                TournamentEndtDate = DateTime.Now.AddMonths(1),
                Status = TournamentStatus.Ongoing,
                Format = TournamentFormat.SixteenTeams
            };
            _context.Tournaments.Add(tournament);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Tournaments/Edit/" + tournament.TournamentId);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Delete_should_return_success_when_tournament_is_found()
        {
            // Arrange
            var tournament = new Tournament
            {
                TournamentId = Guid.NewGuid(),
                TournamentName = "Championship",
                TournamentStartDate = DateTime.Now.AddMonths(-1),
                TournamentEndtDate = DateTime.Now.AddMonths(1),
                Status = TournamentStatus.Ongoing,
                Format = TournamentFormat.SixteenTeams
            };
            _context.Tournaments.Add(tournament);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Tournaments/Delete/" + tournament.TournamentId);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        //POST
        [Fact]
        public async Task Create_should_save_new_tournament()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "TournamentName", "Champions Cup" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Tournaments/Create", content);

            // Assert - Check redirection
            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently);

            // Check if the tournament was created
            var tournament = _context.Tournaments.FirstOrDefault(t => t.TournamentName == "Champions Cup");
            Assert.NotNull(tournament);
            Assert.NotEqual(Guid.Empty, tournament.TournamentId);
            Assert.Equal("Champions Cup", tournament.TournamentName);
        }
        [Fact]
        public async Task Create_should_not_save_new_tournament_with_incorrect_data()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "TournamentName", "" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Tournaments/Create", content);

            // Assert - Check redirection
            response.EnsureSuccessStatusCode();
            Assert.False(_context.Users.Any());
        }
        [Fact]
        public async Task Edit_should_update_tournament()
        {
            // Arrange - Create initial tournament data
            var formValues = new Dictionary<string, string>
            {
                { "TournamentName", "Champions Cup" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act - Create a new tournament
            using var response = await _client.PostAsync("/Tournaments/Create", content);

            // Retrieve tournament from DB
            var tournament = _context.Tournaments.FirstOrDefault();
            Assert.NotNull(tournament);

            // Detach to prevent tracking issues
            _context.Entry(tournament).State = EntityState.Detached;

            // Prepare edit form data
            var editFormValues = new Dictionary<string, string>
            {
                { "TournamentId", tournament.TournamentId.ToString() },
                { "TournamentName", "Updated Cup" },
                { "TournamentStartDate", DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(15).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.Ongoing.ToString() },
                { "Format", TournamentFormat.SixteenTeams.ToString() }
            };

            using var editContent = new FormUrlEncodedContent(editFormValues);

            // Act - Edit the tournament
            using var editResponse = await _client.PostAsync($"/Tournaments/Edit/{tournament.TournamentId}", editContent);

            // Assert
            Assert.True(editResponse.StatusCode == HttpStatusCode.Redirect || editResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Check if the tournament was updated
            var updatedTournament = _context.Tournaments.FirstOrDefault(t => t.TournamentId == tournament.TournamentId);
            Assert.NotNull(updatedTournament);
            Assert.Equal("Updated Cup", updatedTournament.TournamentName);
            Assert.Equal(DateTime.UtcNow.AddDays(1).Date, updatedTournament.TournamentStartDate.Date);
            Assert.Equal(DateTime.UtcNow.AddDays(15).Date, updatedTournament.TournamentEndtDate.Date);
            Assert.Equal(TournamentStatus.Ongoing, updatedTournament.Status);
            Assert.Equal(TournamentFormat.SixteenTeams, updatedTournament.Format);
        }
        [Fact]
        public async Task Delete_should_delete_tournament()
        {
            // Arrange - Create a new tournament
            var formValues = new Dictionary<string, string>
            {
                { "TournamentName", "Champions Cup" },
                { "TournamentStartDate", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "TournamentEndtDate", DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd") },
                { "Status", TournamentStatus.NotStarted.ToString() },
                { "Format", TournamentFormat.EightTeams.ToString() }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act - Create the tournament
            using var response = await _client.PostAsync("/Tournaments/Create", content);

            // Retrieve the tournament from the database
            var tournament = _context.Tournaments.FirstOrDefault();
            Assert.NotNull(tournament);

            // Detach to prevent tracking issues
            _context.Entry(tournament).State = EntityState.Detached;

            // Act - Delete the tournament
            using var deleteResponse = await _client.PostAsync($"/Tournaments/Delete/{tournament.TournamentId}", null);

            // Assert
            // Check for redirect after successful deletion
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.Redirect || deleteResponse.StatusCode == HttpStatusCode.MovedPermanently);

            // Ensure the tournament was deleted from the database
            var deletedTournament = _context.Tournaments.FirstOrDefault(t => t.TournamentId == tournament.TournamentId);
            Assert.Null(deletedTournament);  
        }


    }
}
