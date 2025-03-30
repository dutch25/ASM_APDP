using ASM_APDP.Data;
using ASM_APDP.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM_APDP.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly DatabaseContext _context;
        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }
        public bool CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<User>(); // null
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                return _context.Users.Find(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            try
            {
                return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}

