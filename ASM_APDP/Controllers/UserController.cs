using ASM_APDP.Models;
using ASM_APDP.Facades;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASM_APDP.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserFacade _userFacade;

        public UserController(IUserFacade userFacade)
        {
            _userFacade = userFacade;
        }

        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register (Chỉ tạo Student)
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _userFacade.GetUserByEmailAndPassword(model.Username, model.Password);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // Lưu mật khẩu thô (Không mã hóa)
                    Email = model.Email,
                    CreateDate = DateTime.Now,
                    RoleId = 2 // Luôn là Student
                };

                bool success = _userFacade.RegisterUser(user);
                if (success)
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Error creating user.");
            }
            return View(model);
        }

        // GET: /User/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /User/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userFacade.GetUserByEmailAndPassword(model.Username, model.Password);
                if (user == null || user.Password != model.Password) // Kiểm tra mật khẩu trực tiếp
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }

                // Lưu vào session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("RoleId", user.RoleId);
                HttpContext.Session.SetInt32("IsLogin", 1);

                // Điều hướng dựa trên RoleId
                return user.RoleId switch
                {
                    1 => RedirectToAction("AdminDashboard", "Admin"),
                    2 => RedirectToAction("Index", "Home"),
                    3 => RedirectToAction("TeacherDashboard", "Teacher"),
                    _ => RedirectToAction("Login")
                };
            }
            return View(model);
        }

        // GET: /User/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: /User/Profile
        public async Task<IActionResult> Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                Username = user.Username,
                Email = user.Email
            };

            return View(model);
        }

        // POST: /User/Profile
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = HttpContext.Session.GetString("Username");
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login");
                }

                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                user.Email = model.Email;
                if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmPassword)
                {
                    user.Password = model.NewPassword; // Update password if provided and matches confirmation
                }

                await _userRepository.UpdateUser(user);

                return RedirectToAction("Profile");
            }

            return View(model);
        }




    }
}
