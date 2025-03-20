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
            return View("~/Views/Home/Register.cshtml");
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var users = _userFacade.GetAllUsers();
                return View("~/Views/Home/Index.cshtml");
            }
            catch(Exception e)
            {
                return View(null);
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                return View("~/Views/Home/Login.cshtml");
            }
            catch(Exception e)
            {
                return View(null);
            }
        }
    }
}
