using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();

        User GetUserById(int id);

        User GetUserByUsernameAndPassword(string username, string password);

        bool CreateUser(User user);

        bool UpdateUser(User user);

        bool DeleteUser(int id);
    }
}
