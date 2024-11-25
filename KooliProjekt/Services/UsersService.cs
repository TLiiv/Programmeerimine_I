using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _context;
        public UsersService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> AllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User>Get(Guid id)
        {
            return await _context.Users
                .Include(user => user.UserBets)
                .FirstOrDefaultAsync(u => u.UserId == id);
               
        }

        public async Task Save(User user)
        {
            if (user.UserId == default(Guid))
            {
                user.UserId = Guid.NewGuid();
                _context.Users.Add(user);
            }
            else
            {
                _context.Users.Update(user);
            }
      
            //user.UserId = Guid.NewGuid();<--on ka vaja üle vaadata pärast kas on vaja siia panna või mitte
            //_context.Users.Add(user); kui update ja create lahku lüüa siis töötaks
            //await _context.SaveChangesAsync(); 
        }
        public async Task Delete(Guid id)
        {
            await _context.Users
                .Where(user => user.UserId == id)
                .ExecuteDeleteAsync();
        }
    }
}



