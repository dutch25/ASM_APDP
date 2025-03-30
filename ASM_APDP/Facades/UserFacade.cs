using ASM_APDP.Models;
using ASM_APDP.Repositories;
using ASM_APDP.ViewModels;

namespace ASM_APDP.Facades
{
    public class UserFacade : IUserFacade
    {
        private readonly IUserRepository _userRepository;

        public UserFacade(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _userRepository.GetUserByUsernameAndPassword(username, password);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public bool RegisterUser(User user)
        {
            return _userRepository.CreateUser(user);
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }


        public bool DeleteUser(int id)
        {
            return _userRepository.DeleteUser(id);
        }

        public async Task<ProfileViewModel> GetUserProfileAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return null;

            return new ProfileViewModel
            {
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<bool> UpdateUserProfileAsync(string username, ProfileViewModel model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return false;

            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmPassword)
            {
                user.Password = model.NewPassword; // ⚠️ Consider hashing the password!
            }

            return await _userRepository.UpdateUserAsync(user);
        }
    }
}
