﻿using ASM_APDP.Models;
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
            // Check if username already exists
            if (_userRepository.GetUserByUsername(user.Username) != null)
            {
                return false; // Username already exists
            }

            // Check if email already exists
            if (_userRepository.EmailExists(user.Email))
            {
                return false; // Email already exists
            }

            return _userRepository.CreateUser(user);
        }

        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        public bool EmailExists(string email)
        {
            return _userRepository.EmailExists(email);
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
                Email = user.Email,
                FullName = user.FullName
            };
        }

        public async Task<bool> UpdateUserProfileAsync(string username, ProfileViewModel model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return false;

            bool isUpdated = false;

            if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
            {
                user.Email = model.Email;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(model.FullName) && model.FullName != user.FullName)
            {
                user.FullName = model.FullName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmPassword)
            {
                user.Password = model.NewPassword; // Không mã hóa mật khẩu
                isUpdated = true;
            }

            if (!isUpdated) return false; // Không có gì thay đổi, không cần lưu

            return await _userRepository.UpdateUserAsync(user);
        }

    }
}
