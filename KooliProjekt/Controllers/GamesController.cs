using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;
using KooliProjekt.Search;

namespace KooliProjekt.Controllers
{
    public class GamesController : Controller
    {
        
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
           
            _gamesService = gamesService;
        }

        // GET: Games
        public async Task<IActionResult> Index(GamesSearch search = null)
        {
            var data = await _gamesService.AllGames(search);
            return View(data);
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _gamesService.Get(id.Value);
              
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public async Task<IActionResult> Create()
        {
            var dropdownData = await _gamesService.GetDropdownData();

            ViewData["AwayTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName");
            ViewData["HomeTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName");
            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName");
            
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GamesId,GameStartDate,GameStartTime,HomeTeamId,AwayTeamId,AreTeamsConfirmed,TournamentId")] Game game)
        {
            if (ModelState.IsValid)
            {
               await _gamesService.Save(game);
                return RedirectToAction(nameof(Index));
            }

            var dropdownData = await _gamesService.GetDropdownData();

            ViewData["AwayTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", game.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", game.HomeTeamId);
            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", game.TournamentId);
            return View(game);
            //Console.WriteLine($"Received TournamentId: {game.TournamentId}");

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
                  
            //    await _gamesService.Save(game);

            //        return RedirectToAction(nameof(Index));
            //    }
            //    catch (Exception ex)
            //    {                 
            //        Console.WriteLine($"Error saving game: {ex.Message}");
            //        return View(game);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("ModelState is not valid");

            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            //    }
            //    ViewData["AwayTeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", game.AwayTeamId);
            //    ViewData["HomeTeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", game.HomeTeamId);
            //    ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName", game.TournamentId);
                
            //    return View(game);
            //}
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _gamesService.Get(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            var dropdownData = await _gamesService.GetDropdownData();
            ViewData["AwayTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", game.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", game.HomeTeamId);
            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", game.TournamentId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GamesId,GameStartDate,GameStartTime,HomeTeamId,AwayTeamId,AreTeamsConfirmed,TournamentId")] Game game)
        {
            if (id != game.GamesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _gamesService.Save(game);
                return RedirectToAction(nameof(Index));
            }

            var dropdownData = await _gamesService.GetDropdownData();
            ViewData["AwayTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", game.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", game.HomeTeamId);
            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", game.TournamentId);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _gamesService.Get(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _gamesService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
