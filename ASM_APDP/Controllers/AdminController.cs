using Microsoft.AspNetCore.Mvc;

namespace ASM_APDP.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
