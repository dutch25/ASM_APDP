using ASM_APDP.Models;
using ASM_APDP.Facades;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ASM_APDP.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserFacade _userFacade;
        private readonly IRoleFacade _roleFacade;

        public UserController(IUserFacade userFacade, IRoleFacade roleFacade)
        {
            _userFacade = userFacade;
            _roleFacade = roleFacade;
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
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // Không mã hóa mật khẩu
                    Email = model.Email,
                    CreateDate = DateTime.Now,
                    DoB = model.DoB,
                    RoleId = 2 // Luôn là Student
                };

                bool success = _userFacade.RegisterUser(user);
                if (!success)
                {
                    if (_userFacade.GetUserByUsername(model.Username) != null)
                    {
                        ModelState.AddModelError("", "Username already exists.");
                    }
                    else if (_userFacade.EmailExists(model.Email))
                    {
                        ModelState.AddModelError("", "Email already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error creating user.");
                    }
                    return View(model);
                }

                return RedirectToAction("Login");
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
                var user = _userFacade.GetUserByUsernameAndPassword(model.Username, model.Password);
                if (user == null || user.Password != model.Password) // Kiểm tra mật khẩu trực tiếp
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }

                var role = _roleFacade.GetRoleById(user.RoleId);
                if (role == null || string.IsNullOrEmpty(role.RoleName) || role.RoleName != model.RoleName)
                {
                    ModelState.AddModelError("", "Invalid role assignment.");
                    return View(model);
                }

                model.RoleName = role.RoleName;

                // Lưu vào session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("RoleId", user.RoleId);
                HttpContext.Session.SetString("RoleName", model.RoleName);
                HttpContext.Session.SetInt32("IsLogin", 1);

                // Điều hướng dựa trên RoleId
                return role.RoleName switch
                {
                    "Admin" => RedirectToAction("AdminDashboard", "Admin"),
                    "Student" => RedirectToAction("Index", "Home"),
                    "Teacher" => RedirectToAction("TeacherDashboard", "Teacher"),
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

            var model = await _userFacade.GetUserProfileAsync(username);
            if (model == null)
            {
                return RedirectToAction("Login");
            }

            return View(model);
        }

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

                bool success = await _userFacade.UpdateUserProfileAsync(username, model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Your profile has been updated successfully.";

                    return model.RoleId switch
                    {
                        1 => RedirectToAction("AdminDashboard", "Admin"),
                        2 => RedirectToAction("Index", "Home"),
                        3 => RedirectToAction("TeacherDashboard", "Teacher"),
                        _ => RedirectToAction("Profile", "User")
                    };
                }
                ModelState.AddModelError("", "Error updating user.");
            }

            return View(model);
        }

    }
}
