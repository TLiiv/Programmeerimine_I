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
    public class TournamentsController : Controller
    {
    

        private readonly ITournamentsService _tournamentsService;

        public TournamentsController(ITournamentsService tournamentsService)
        {
           
            _tournamentsService = tournamentsService;
        }

        // GET: Tournaments
        public async Task<IActionResult> Index()
        {
            var data = await _tournamentsService.AllTournaments();

            return View(data);
        }

        // GET: Tournaments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _tournamentsService.Get(id.Value);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        // GET: Tournaments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TournamentId,TournamentName,TournamentStartDate,TournamentEndtDate,Status,Format")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                await _tournamentsService.Save(tournament);

               
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _tournamentsService.Get(id.Value);
            if (tournament == null)
            {
                return NotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TournamentId,TournamentName,TournamentStartDate,TournamentEndtDate,Status,Format")] Tournament tournament)
        {
            if (id != tournament.TournamentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                await _tournamentsService.Save(tournament);  
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _tournamentsService.Get(id.Value);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
             await _tournamentsService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
