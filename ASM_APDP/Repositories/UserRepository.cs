using ASM_APDP.Data;
using ASM_APDP.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null) return false;

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return _context.SaveChanges() > 0;
        }
    }
}
