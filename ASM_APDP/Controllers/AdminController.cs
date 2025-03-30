using Microsoft.AspNetCore.Mvc;

namespace ASM_APDP.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }
        public IActionResult AdminProfile()
        {
            return View();
        }

        public IActionResult CourseManagement()
        {
            return View();
        }
        public IActionResult AssignCoursesToStudents()
        {
            return View();
        }
        public IActionResult ViewAllAssignCourses()
        {
            return View();
        }

        public IActionResult ViewAllCourses()
        {
            return View();
        }
    }
}
