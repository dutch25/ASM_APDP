using ASM_APDP.Models;
using ASM_APDP.Repositories;

namespace ASM_APDP.Facades
{
    public class UserFacade : IUserFacade
    {
        private const string DEFAULT_ROLE = "User"; // Assuming a default role name
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserFacade(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        int IUserFacade.DeleteUser(int id)
        {
            try
            {
                return _userRepository.Delete(id);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        IEnumerable<User> IUserFacade.GetAllUsers()
        {
            try
            {
                return _userRepository.GetAll();
            }
            catch (Exception)
            {
                return new List<User>();
            }
        }

        User IUserFacade.getUserByID(int id)
        {
            try
            {
                return _userRepository.getUserById(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        User IUserFacade.RegisterUser(string username, string password, string email)
        {
            try
            {
                var role = _roleRepository.GetRoleByName(DEFAULT_ROLE);
                if (role == null)
                {
                    return null;
                }
                var user = new User
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    RoleId = role.Id,
                    CreateDate = DateTime.Now
                };
                var result = _userRepository.Create(user);
                if (result == 0)
                {
                    return null;
                }
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        User IUserFacade.UpdateUser(User user)
        {
            try
            {
                var result = _userRepository.Update(user);
                if (result == 0)
                {
                    return null;
                }
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
