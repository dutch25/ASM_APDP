using Microsoft.AspNetCore.Mvc;

namespace ASM_APDP.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult TeacherDashboard()
        {
            return View();
        }
    }
}
