using ASM_APDP.Models;
using ASM_APDP.Repositories;

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

        public User GetUserByEmailAndPassword(string username, string password)
        {
            return _userRepository.GetUserByUsernameAndPassword(username, password);
        }

        public bool RegisterUser(User user)
        {
            return _userRepository.CreateUser(user);
        }

        public bool UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        public bool DeleteUser(int id)
        {
            return _userRepository.DeleteUser(id);
        }
    }
}
