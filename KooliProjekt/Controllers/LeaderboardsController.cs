using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class LeaderboardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Leaderboards
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Leaderboards.Include(l => l.Tournament).Include(l => l.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Leaderboards/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _context.Leaderboards
                .Include(l => l.Tournament)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LeaderBoardId == id);
            if (leaderboard == null)
            {
                return NotFound();
            }

            return View(leaderboard);
        }

        // GET: Leaderboards/Create
        public IActionResult Create()
        {
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
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
                leaderboard.LeaderBoardId = Guid.NewGuid();
                _context.Add(leaderboard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName", leaderboard.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", leaderboard.UserId);
            return View(leaderboard);
        }

        // GET: Leaderboards/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _context.Leaderboards.FindAsync(id);
            if (leaderboard == null)
            {
                return NotFound();
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName", leaderboard.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", leaderboard.UserId);
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
                try
                {
                    _context.Update(leaderboard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaderboardExists(leaderboard.LeaderBoardId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName", leaderboard.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", leaderboard.UserId);
            return View(leaderboard);
        }

        // GET: Leaderboards/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _context.Leaderboards
                .Include(l => l.Tournament)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LeaderBoardId == id);
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
            var leaderboard = await _context.Leaderboards.FindAsync(id);
            if (leaderboard != null)
            {
                _context.Leaderboards.Remove(leaderboard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaderboardExists(Guid id)
        {
            return _context.Leaderboards.Any(e => e.LeaderBoardId == id);
        }
    }
}
