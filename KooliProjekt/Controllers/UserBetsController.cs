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
    public class UserBetsController : Controller
    {
        
        private readonly IUserBetsService _userBetsService;

        public UserBetsController(IUserBetsService userBetsService)
        {
            
            _userBetsService = userBetsService;
        }

        // GET: UserBets
        public async Task<IActionResult> Index()
        {
            var data = await _userBetsService.AllUserBets();
            return View(data);
        }

        // GET: UserBets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _userBetsService.Get(id.Value);
              
            if (userBets == null)
            {
                return NotFound();
            }

            return View(userBets);
        }

        // GET: UserBets/Create
        public async Task<IActionResult> Create()
        {
            var dropdownData = await _userBetsService.GetDropdownData();
           
            ViewData["GameId"] = new SelectList(dropdownData.Games, "GamesId", "GamesId");
            ViewData["PredictedWinningTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName");
            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName");
            ViewData["UserId"] = new SelectList(dropdownData.Users, "UserId", "Email");

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

                await _userBetsService.Save(userBets);
                return RedirectToAction(nameof(Index));
            }

            var dropdownData = await _userBetsService.GetDropdownData();

            ViewData["GameId"] = new SelectList(dropdownData.Games, "GamesId", "GamesId", userBets.GameId);
            ViewData["PredictedWinningTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", userBets.PredictedWinningTeamId);
            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", userBets.TournamentId);
            ViewData["UserId"] = new SelectList(dropdownData.Users, "UserId", "Email", userBets.UserId);
            
            return View(userBets);
        }


        // GET: UserBets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _userBetsService.Get(id.Value);
            if (userBets == null)
            {
                return NotFound();
            }

            var dropdownData = await _userBetsService.GetDropdownData();
            ViewData["GameId"] = new SelectList(dropdownData.Games, "GamesId", "GamesId", userBets.GameId);
            ViewData["PredictedWinningTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", userBets.PredictedWinningTeamId);
            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", userBets.TournamentId);
            ViewData["UserId"] = new SelectList(dropdownData.Users, "UserId", "Email", userBets.UserId);
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
               await _userBetsService.Save(userBets);
            }

            var dropdownData = await _userBetsService.GetDropdownData();
            ViewData["GameId"] = new SelectList(dropdownData.Games, "GamesId", "GamesId", userBets.GameId);
            ViewData["PredictedWinningTeamId"] = new SelectList(dropdownData.Teams, "TeamId", "TeamName", userBets.PredictedWinningTeamId);
            ViewData["TournamentId"] = new SelectList(dropdownData.Tournaments, "TournamentId", "TournamentName", userBets.TournamentId);
            ViewData["UserId"] = new SelectList(dropdownData.Users, "UserId", "Email", userBets.UserId);
            return View(userBets);
        }

        // GET: UserBets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _userBetsService.Get(id.Value);
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
            await _userBetsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
