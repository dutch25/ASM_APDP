using ASM_APDP.Models;

namespace ASM_APDP.Facades
{
    public interface IUserFacade
    {
        User RegisterUser(string username, string password, string email);
        User getUserByID(int id);
        IEnumerable<User> GetAllUsers();
        User UpdateUser(User user);
        int DeleteUser(int id);
    }
}
