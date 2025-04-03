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
            var classes = _classFacade.GetAllClasses()?.ToList() ?? new List<Class>();
            var marks = await _markFacade.GetAllMarksAsync() ?? new List<Mark>();

            var studentMarkViewModels = from c in classes
                                        where c.UserID.HasValue && c.UserID.Value > 0
                                        join m in marks
                                        on c.ClassID equals m.ClassID into markGroup
                                        from m in markGroup.DefaultIfEmpty()
                                        select new StudentMarkViewModel
                                        {
                                            UserId = c.UserID.Value,
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

        // POST: /Teacher/UpdateMarkInline
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMarkInline(int? markId, int? userId, int? courseId, int? classId, decimal? grade)
        {
            if (!userId.HasValue || !courseId.HasValue || !classId.HasValue)
            {
                return Json(new { success = false, message = "Invalid input data: UserID, CourseID, or ClassID is missing." });
            }

            try
            {
                // Validate Class assignment exists
                var classAssignment = _classFacade.GetAllClasses()
                    ?.FirstOrDefault(c => c.UserID == userId.Value && c.CourseID == courseId.Value && c.ClassID == classId.Value);
                if (classAssignment == null)
                {
                    return Json(new { success = false, message = $"No class assignment found for UserID={userId}, CourseID={courseId}, ClassID={classId}." });
                }

                if (markId.HasValue && markId.Value > 0)
                {
                    // Update existing mark
                    var mark = _markFacade.GetMarkById(markId.Value);
                    if (mark == null)
                    {
                        return Json(new { success = false, message = "Mark not found." });
                    }
                    mark.Grade = grade;
                    var success = _markFacade.UpdateMark(mark);
                    return Json(new { success = success, message = success ? "Mark updated successfully." : "Error updating mark." });
                }
                else
                {
                    // Check if a Mark already exists for this combination
                    var existingMark = _markFacade.GetAllMarksAsync().Result
                        ?.FirstOrDefault(m => m.UserID == userId.Value && m.CourseID == courseId.Value && m.ClassID == classId.Value);

                    if (existingMark != null)
                    {
                        // Update existing mark if found
                        existingMark.Grade = grade;
                        var success = _markFacade.UpdateMark(existingMark);
                        return Json(new { success = success, message = success ? "Mark updated successfully." : "Error updating mark.", markId = existingMark.MarkID });
                    }
                    else
                    {
                        // Create a new mark
                        var newMark = new Mark
                        {
                            UserID = userId.Value,
                            CourseID = courseId.Value,
                            ClassID = classId.Value,
                            Grade = grade
                        };
                        var rowsAffected = _markFacade.CreateMark(newMark);
                        if (rowsAffected > 0)
                        {
                            var createdMark = _markFacade.GetAllMarksAsync().Result
                                .FirstOrDefault(m => m.UserID == userId.Value && m.CourseID == courseId.Value && m.ClassID == classId.Value);
                            return Json(new { success = true, message = "Mark created successfully.", markId = createdMark?.MarkID });
                        }
                        return Json(new { success = false, message = "Failed to create mark. Check database constraints." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}