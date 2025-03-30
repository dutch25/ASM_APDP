using ASM_APDP.Data;
using ASM_APDP.Models;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM_APDP.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register (Chỉ tạo Student)
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
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

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

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
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
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
    }
}
