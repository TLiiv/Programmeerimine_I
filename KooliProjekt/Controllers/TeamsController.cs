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
    public class TeamsController : Controller
    {

        private readonly ITeamsService _teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        // GET: Teams
        public async Task<IActionResult> Index(TeamsSearch search = null)
        {
            var data = await _teamsService.AllTeams(search);

            return View(data);
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _teamsService.Get(id.Value);
                
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamId,TeamName,GoalsScored,GoalsScoredAgainstThem,GamesWon,GamesLost,GamesPlayed")] Team team)
        {
            if (ModelState.IsValid)
            {
               await _teamsService.Save(team);
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _teamsService.Get(id.Value);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TeamId,TeamName,GoalsScored,GoalsScoredAgainstThem,GamesWon,GamesLost,GamesPlayed")] Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _teamsService.Save(team);
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _teamsService.Get(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _teamsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    
    }
}
