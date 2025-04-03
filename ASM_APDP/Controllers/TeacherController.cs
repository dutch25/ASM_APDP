using ASM_APDP.Facades;
using ASM_APDP.Models;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_APDP.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IMarkFacade _markFacade;
        private readonly IClassFacade _classFacade;

        public TeacherController(IMarkFacade markFacade, IClassFacade classFacade)
        {
            _markFacade = markFacade;
            _classFacade = classFacade ?? throw new ArgumentNullException(nameof(classFacade));
        }

        public IActionResult TeacherDashboard()
        {
            return View();
        }

        // GET: /Teacher/ReportMark
        public async Task<IActionResult> ReportMark()
        {
            // Lấy danh sách tất cả lớp học đã gán học sinh từ ClassFacade
            var classes = _classFacade.GetAllClasses()?.ToList() ?? new List<Class>();
            var marks = await _markFacade.GetAllMarksAsync();

            // Kết hợp dữ liệu từ Class và Mark, lọc bỏ UserID = 0 hoặc null
            var studentMarkViewModels = from c in classes
                                        where c.UserID.HasValue && c.UserID.Value > 0 // Lọc bỏ UserID null hoặc 0
                                        join m in marks
                                        on c.ClassID equals m.ClassID into markGroup
                                        from m in markGroup.DefaultIfEmpty()
                                        select new StudentMarkViewModel
                                        {
                                            UserId = c.UserID.Value, // Sử dụng .Value vì đã lọc null
                                            StudentName = c.User?.FullName ?? "N/A",
                                            CourseName = c.Course?.CourseName ?? "N/A",
                                            ClassName = c.ClassName ?? "N/A",
                                            MarkId = m?.MarkID,
                                            Grade = m?.Grade,
                                            CourseId = c.CourseID ?? 0,
                                            ClassId = c.ClassID
                                        };

            return View(studentMarkViewModels.ToList());
        }

        // GET: /Teacher/EditMark/5
        public IActionResult EditMark(int? id, int? userId, int? courseId, int? classId)
        {
            Mark mark;
            if (id.HasValue)
            {
                mark = _markFacade.GetMarkById(id.Value);
                if (mark == null)
                {
                    return NotFound();
                }
            }
            else
            {
                if (!userId.HasValue || !courseId.HasValue || !classId.HasValue)
                {
                    return BadRequest("UserId, CourseId, and ClassId are required to create a new mark.");
                }

                mark = new Mark
                {
                    UserID = userId.Value,
                    CourseID = courseId.Value,
                    ClassID = classId.Value,
                    Grade = 0 // Giá trị mặc định, hoặc null nếu Grade là decimal?
                };
            }
            return View(mark);
        }

        // POST: /Teacher/EditMark/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMark(int id, [Bind("MarkID,UserID,CourseID,ClassID,Grade")] Mark mark)
        {
            if (id != mark.MarkID && id != 0) // id = 0 nếu là Mark mới
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) // Tạo mới Mark
                    {
                        _markFacade.CreateMark(mark);
                    }
                    else // Cập nhật Mark
                    {
                        _markFacade.UpdateMark(mark);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_markFacade.GetMarkById(mark.MarkID) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ReportMark));
            }
            return View(mark);
        }
    }
}