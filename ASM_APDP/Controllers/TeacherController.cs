using ASM_APDP.Data;
using ASM_APDP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_APDP.Controllers
{
    public class TeacherController : Controller
    {
        private readonly DatabaseContext _context;

        public TeacherController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult TeacherDashboard()
        {
            return View();
        }

        // GET: /Teacher/ReportMark
        public async Task<IActionResult> ReportMark()
        {
            var marks = await _context.Marks
                .Include(m => m.User)
                .ThenInclude(u => u.Classes) // Include thông tin về lớp học
                .Include(m => m.Course)
                .ToListAsync();

            return View(marks);
        }
    }
}

