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
    public class UserBetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserBetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserBets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UsersBets.Include(u => u.Game).Include(u => u.PredictedWinningTeam).Include(u => u.Tournament).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserBets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _context.UsersBets
                .Include(u => u.Game)
                .Include(u => u.PredictedWinningTeam)
                .Include(u => u.Tournament)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBets == null)
            {
                return NotFound();
            }

            return View(userBets);
        }

        // GET: UserBets/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GamesId", "GamesId");
            ViewData["PredictedWinningTeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName");
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: UserBets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TournamentId,GameId,PredictedWinningTeamId,PredictedHomeGoals,PredictedAwayGoals,AccountBalance,BetAmount,BetPlacedDate")] UserBets userBets)
        {
            if (ModelState.IsValid)
            {
                userBets.Id = Guid.NewGuid();
                _context.Add(userBets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GamesId", "GamesId", userBets.GameId);
            ViewData["PredictedWinningTeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", userBets.PredictedWinningTeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName", userBets.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", userBets.UserId);
            return View(userBets);
        }

        // GET: UserBets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _context.UsersBets.FindAsync(id);
            if (userBets == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GamesId", "GamesId", userBets.GameId);
            ViewData["PredictedWinningTeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", userBets.PredictedWinningTeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName", userBets.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", userBets.UserId);
            return View(userBets);
        }

        // POST: UserBets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId,TournamentId,GameId,PredictedWinningTeamId,PredictedHomeGoals,PredictedAwayGoals,AccountBalance,BetAmount,BetPlacedDate")] UserBets userBets)
        {
            if (id != userBets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBetsExists(userBets.Id))
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
            ViewData["GameId"] = new SelectList(_context.Games, "GamesId", "GamesId", userBets.GameId);
            ViewData["PredictedWinningTeamId"] = new SelectList(_context.Teams, "TeamId", "TeamName", userBets.PredictedWinningTeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentName", userBets.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", userBets.UserId);
            return View(userBets);
        }

        // GET: UserBets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _context.UsersBets
                .Include(u => u.Game)
                .Include(u => u.PredictedWinningTeam)
                .Include(u => u.Tournament)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBets == null)
            {
                return NotFound();
            }

            return View(userBets);
        }

        // POST: UserBets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userBets = await _context.UsersBets.FindAsync(id);
            if (userBets != null)
            {
                _context.UsersBets.Remove(userBets);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBetsExists(Guid id)
        {
            return _context.UsersBets.Any(e => e.Id == id);
        }
    }
}
