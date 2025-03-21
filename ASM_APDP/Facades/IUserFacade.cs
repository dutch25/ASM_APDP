using ASM_APDP.Models;

namespace ASM_APDP.Facades
{
    public interface IUserFacade
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByEmailAndPassword(string email, string password);
        bool RegisterUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(int id);
    }
}
