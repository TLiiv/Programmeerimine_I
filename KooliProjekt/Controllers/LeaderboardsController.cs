using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class LeaderboardsController : Controller
    {
      
        private readonly ILeaderboardsService _leaderboardsService;

        public LeaderboardsController(ILeaderboardsService leaderboardsService)
        {
            _leaderboardsService = leaderboardsService;
        }

        // GET: Leaderboards
        public async Task<IActionResult> Index()
        {
            var data =await _leaderboardsService.AllLeaderboards();
            return View(data);
        }

        // GET: Leaderboards/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _leaderboardsService.Get(id.Value);
            if (leaderboard == null)
            {
                return NotFound();
            }

            return View(leaderboard);
        }

        // GET: Leaderboards/Create
        public async Task<IActionResult> Create()
        {
            var dropdownData = await _leaderboardsService.GetDropdownData();

            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName");
            ViewData["UserId"] = new SelectList(dropdownData.Users, "UserId", "Email");
            return View();
        }

        // POST: Leaderboards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaderBoardId,UserId,TournamentId,PredictedPoints,Rank")] Leaderboard leaderboard)
        {
            if (ModelState.IsValid)
            {
                await _leaderboardsService.Save(leaderboard);
                return RedirectToAction(nameof(Index));
            }
            var dropdownData = await _leaderboardsService.GetDropdownData();

            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", leaderboard.TournamentId);
            ViewData["UserId"] = new SelectList(dropdownData.Users, "UserId", "Email", leaderboard.UserId);
            return View(leaderboard);
        }

        // GET: Leaderboards/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _leaderboardsService.Get(id.Value);
            if (leaderboard == null)
            {
                return NotFound();
            }

            var dropdownData = await _leaderboardsService.GetDropdownData();

            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", leaderboard.TournamentId);
            ViewData["UserId"] = new SelectList(dropdownData.Users, "UserId", "Email", leaderboard.UserId);
            return View(leaderboard);
        }

        // POST: Leaderboards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LeaderBoardId,UserId,TournamentId,PredictedPoints,Rank")] Leaderboard leaderboard)
        {
            if (id != leaderboard.LeaderBoardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _leaderboardsService.Save(leaderboard);
                return RedirectToAction(nameof(Index));
            }
            var dropdownData = await _leaderboardsService.GetDropdownData();


            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", leaderboard.TournamentId);
            ViewData["UserId"] = new SelectList(dropdownData.Users, "UserId", "Email", leaderboard.UserId);
            return View(leaderboard);
        }

        // GET: Leaderboards/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _leaderboardsService.Get(id.Value);
            if (leaderboard == null)
            {
                return NotFound();
            }

            return View(leaderboard);
        }

        // POST: Leaderboards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _leaderboardsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
