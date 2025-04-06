using ASM_APDP.Factories; // Thay Facades bằng Factories cho Mark
using ASM_APDP.Models;
using ASM_APDP.Repositories;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using ASM_APDP.Facades;

namespace ASM_APDP.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMarkFactory _markFactory; // Thay IMarkFacade bằng IMarkFactory
        private readonly IClassFacade _classFacade;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository, IMarkFactory markFactory, IClassFacade classFacade)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _markFactory = markFactory ?? throw new ArgumentNullException(nameof(markFactory));
            _classFacade = classFacade ?? throw new ArgumentNullException(nameof(classFacade));
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
                    FullName = model.FullName,
                    Password = model.Password,
                    Email = model.Email,
                    CreateDate = DateTime.Now,
                    DoB = model.DoB,
                    RoleId = 2 // Luôn là Student
                };

                bool success = _userRepository.CreateUser(user);
                if (!success)
                {
                    if (_userRepository.GetUserByUsername(model.Username) != null)
                    {
                        ModelState.AddModelError("", "Username already exists.");
                    }
                    else if (_userRepository.EmailExists(model.Email))
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
                var user = _userRepository.GetUserByUsernameAndPassword(model.Username, model.Password);
                if (user == null || user.Password != model.Password)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }

                var role = _roleRepository.GetRoleByID(user.RoleId);
                if (role == null || string.IsNullOrEmpty(role.RoleName) || role.RoleName != model.RoleName)
                {
                    ModelState.AddModelError("", "Invalid role assignment.");
                    return View(model);
                }

                model.RoleName = role.RoleName;

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("RoleId", user.RoleId);
                HttpContext.Session.SetString("RoleName", model.RoleName);
                HttpContext.Session.SetInt32("IsLogin", 1);

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

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                DoB = user.DoB,
                RoleId = user.RoleId
            };

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

                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // Kiểm tra mật khẩu và xác nhận mật khẩu
                if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Password and Confirm Password do not match.");
                    return View(model);
                }

                // Cập nhật các trường từ model
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.DoB = model.DoB;
                if (!string.IsNullOrEmpty(model.NewPassword)) // Chỉ cập nhật Password nếu người dùng nhập giá trị mới
                {
                    user.Password = model.NewPassword;
                }

                bool success = await _userRepository.UpdateUserAsync(user);
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
        public IActionResult ViewCourse()
        {
            return View();
        }

        public async Task<IActionResult> ViewMark()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var roleName = HttpContext.Session.GetString("RoleName");

            if (!userId.HasValue || roleName != "Student")
            {
                return RedirectToAction("Login");
            }

            var allClasses = _classFacade?.GetAllClasses();
            var classes = allClasses != null
                ? allClasses.Where(c => c.UserID == userId.Value).ToList()
                : new List<Class>();

            var marks = _markFactory.GetAllMarks().ToList(); // Sử dụng IMarkFactory

            var studentMarks = from c in classes
                               join m in marks
                               on c.ClassID equals m.ClassID into markGroup
                               from m in markGroup.DefaultIfEmpty()
                               select new StudentMarkViewModel
                               {
                                   ClassName = c.ClassName ?? "N/A",
                                   CourseName = c.Course?.CourseName ?? "N/A",
                                   Grade = m?.Grade
                               };

            var markList = studentMarks.ToList();
            if (!markList.Any())
            {
                ViewBag.NoDataMessage = "No marks available yet.";
            }

            return View(markList);
        }
    }
}