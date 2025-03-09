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
    [ApiController]
    [Route("Users")]
    public class UsersController : Controller
    {   
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            
            _usersService = usersService;
        }

        // GET: Users
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data  = await _usersService.AllUsers();
            return View(data);
            
        }

       

        [HttpGet("Details/{id}")]
        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _usersService.Get(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpGet("Details/Create")]
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,FirstName,LastName,Email,Password,PhoneNumber,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                
                await _usersService.Save(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }



        [HttpGet("Edit/{id}")]
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _usersService.Get(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost("Edit/{id}")]
        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,UserName,FirstName,LastName,Email,Password,PhoneNumber,IsAdmin")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _usersService.Save(user);
            }
            return View(user);
        }
        [HttpGet("Delete/{id}")]
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _usersService.Get(id.Value);
                
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _usersService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        //API ENDPOINTS

        [HttpGet("api/allusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var data = await _usersService.AllUsers();
            return Ok(data);
        }

        [HttpPost("api/save")]
        public async Task<IActionResult> Save([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }


            await _usersService.Save(user);

            return Ok(user);
        }
        [HttpPut("api/update/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] User user)
        {
            if (userId != user.UserId)
            {
                return BadRequest("User ID in URL does not match ID in body.");
            }

            //var existingUser = await _usersService.Get(userId);
            //if (existingUser == null)
            //{
            //    return NotFound($"User with ID {userId} not found.");
            //}

            await _usersService.Save(user);
            return NoContent(); // Return 204 for a successful update without content.
        }

       

        [HttpDelete("api/delete/{id}")] 
        public async Task<IActionResult> Delete(Guid id)
        {
            await _usersService.Delete(id);
           
            return Ok(); 
        }
    }
}
