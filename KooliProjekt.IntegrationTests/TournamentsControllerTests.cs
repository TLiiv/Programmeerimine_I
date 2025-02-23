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
    public class TournamentsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        public TournamentsControllerTests()
        {
            _client = Factory.CreateClient();
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
    }
}
