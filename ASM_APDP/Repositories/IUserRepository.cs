using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();

        User GetUserById(int id);
        Task<User> GetUserByUsernameAsync(string username);
        bool EmailExists(string email);
        User GetUserByUsernameAndPassword(string username, string password);

        User GetUserByEmail(string email);

        User GetUserByUsername(string username);
        bool CreateUser(User user);

        Task<bool> UpdateUserAsync(User user);


        bool DeleteUser(int id);
    }
}
