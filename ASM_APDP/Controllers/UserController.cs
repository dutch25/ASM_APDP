using ASM_APDP.Models;
using ASM_APDP.Facades;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ASM_APDP.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserFacade _userFacade;
        private readonly IRoleFacade _roleFacade;
        private readonly IMarkFacade _markFacade;
        private readonly IClassFacade _classFacade;

        public UserController(IUserFacade userFacade, IRoleFacade roleFacade, IMarkFacade markFacade, IClassFacade classFacade)
        {
            _userFacade = userFacade;
            _roleFacade = roleFacade;
            _markFacade = markFacade;
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

            // Fetch classes with null check
            var allClasses = _classFacade?.GetAllClasses(); // Null check on _classFacade
            var classes = allClasses != null
                ? allClasses.Where(c => c.UserID == userId.Value).ToList()
                : new List<Class>();

            // Fetch marks with null check and exception handling
            List<Mark> marks = new List<Mark>();
            if (_markFacade != null)
            {
                try
                {
                    marks = await _markFacade.GetAllMarksAsync() ?? new List<Mark>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error fetching marks: {ex.Message}");
                }
            }

            // Join data safely
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
