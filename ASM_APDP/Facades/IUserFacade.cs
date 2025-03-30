using ASM_APDP.Models;
using ASM_APDP.ViewModels;

namespace ASM_APDP.Facades
{
    public interface IUserFacade
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByUsernameAndPassword(string email, string password);
        bool RegisterUser(User user);
        Task UpdateUser(User user);
        bool DeleteUser(int id);

        Task<ProfileViewModel> GetUserProfileAsync(string username);
        Task<bool> UpdateUserProfileAsync(string username, ProfileViewModel model);
    }
}
