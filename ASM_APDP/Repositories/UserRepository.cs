using ASM_APDP.Data;
using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly DatabaseContext _context;
        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }
        int IUserRepository.Create(User user)
        {
            try
            {
                _context.Users.Add(user);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        int IUserRepository.Delete(int id)
        {
            try
            {
                var user = _context.Users.Find(id);
                _context.Users.Remove(user);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        IEnumerable<User> IUserRepository.GetAll()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        User IUserRepository.getUserById(int id)
        {
            try
            {
                return _context.Users.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        int IUserRepository.Update(User user)
        {
            try
            {
                _context.Users.Update(user);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}

