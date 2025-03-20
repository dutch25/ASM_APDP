using ASM_APDP.Facades;
using ASM_APDP.Models;
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
        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            try
            {
                var _user = _userFacade.RegisterUser(user.Username, user.Password, user.Email);
                return View("~/Views/Home/Register.cshtml", _user);
            }
            catch (Exception e)
            {
                return View("~/Views/Home/Register.cshtml", null);
            }   
            
        }
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View("~/Views/Home/Register.cshtml", null);
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var users = _userFacade.GetAllUsers();
                return View("~/View/Home/Login.cshtml");
            }
            catch(Exception e)
            {
                return View(null);
            }
        }

        [HttpPost]
        public IActionResult LoginUser(User user)
        {
            try
            {
                var _user = _userFacade.GetAllUsers().FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password); ;
                if(_user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                    return View("~/Views/Home/Login.cshtml", null);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An Error Occurred While Processing Your Request.");
                return View("~/Views/Home/Login.cshtml", null);
            }
            
        }
    }
}
