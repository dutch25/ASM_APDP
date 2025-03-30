using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();

        User GetUserById(int id);
        Task<User> GetUserByUsernameAsync(string username);
        User GetUserByUsernameAndPassword(string username, string password);

        bool CreateUser(User user);

        Task UpdateUser(User user);

        bool DeleteUser(int id);
    }
}
