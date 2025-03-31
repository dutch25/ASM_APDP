using ASM_APDP.Data;
using ASM_APDP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace ASM_APDP.Controllers
{
    public class AdminController : Controller
    {
        private readonly DatabaseContext _context;
        public AdminController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult AdminDashboard()
        {
            return View();
        }

        // GET: /Admin/Profile
        public IActionResult AdminProfile()
        {
            return View();
        }

        // GET: /Admin/ManageCourses
        public IActionResult CourseManagement()
        {
            return View();
        }

        // GET: /Admin/AssignCoursesToStudents
        public IActionResult AssignCoursesToStudents()
        {
            return View();
        }

        // GET: /Admin/ViewAllAssignCourses
        public IActionResult ViewAllAssignCourses()
        {
            return View();
        }

        // GET: /Admin/ViewAllCourses
        public async Task<IActionResult> ViewAllCourses()
        {
            var courses = await _context.Courses
        .Select(c => new { c.CourseID, c.CourseName })
        .ToListAsync();
            return View(courses);
        }
    }
}
