using ASM_APDP.Facades;
using ASM_APDP.Factories; // Thay Facades bằng Factories cho Mark
using ASM_APDP.Models;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_APDP.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IMarkFactory _markFactory; // Thay IMarkFacade bằng IMarkFactory
        private readonly IClassFacade _classFacade;

        public TeacherController(IMarkFactory markFactory, IClassFacade classFacade)
        {
            _markFactory = markFactory ?? throw new ArgumentNullException(nameof(markFactory));
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
            var marks = _markFactory.GetAllMarks().ToList(); // Sử dụng IMarkFactory

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
                TempData["Message"] = "Invalid input data: UserID, CourseID, or ClassID is missing.";
                TempData["Success"] = false;
                return RedirectToAction("ReportMark");
            }

            try
            {
                var classAssignment = _classFacade.GetAllClasses()
                    ?.FirstOrDefault(c => c.UserID == userId.Value && c.CourseID == courseId.Value && c.ClassID == classId.Value);
                if (classAssignment == null)
                {
                    TempData["Message"] = $"No class assignment found for UserID={userId}, CourseID={courseId}, ClassID={classId}.";
                    TempData["Success"] = false;
                    return RedirectToAction("ReportMark");
                }

                if (markId.HasValue && markId.Value > 0)
                {
                    var mark = _markFactory.GetMarkById(markId.Value); // Sử dụng IMarkFactory
                    if (mark == null)
                    {
                        TempData["Message"] = "Mark not found.";
                        TempData["Success"] = false;
                        return RedirectToAction("ReportMark");
                    }
                    mark.Grade = grade;
                    var success = _markFactory.UpdateMark(mark); // Sử dụng IMarkFactory
                    TempData["Message"] = success ? "Mark updated successfully." : "Error updating mark.";
                    TempData["Success"] = success;
                }
                else
                {
                    var existingMark = _markFactory.GetAllMarks()
                        .FirstOrDefault(m => m.UserID == userId.Value && m.CourseID == courseId.Value && m.ClassID == classId.Value);

                    if (existingMark != null)
                    {
                        existingMark.Grade = grade;
                        var success = _markFactory.UpdateMark(existingMark); // Sử dụng IMarkFactory
                        TempData["Message"] = success ? "Mark updated successfully." : "Error updating mark.";
                        TempData["Success"] = success;
                    }
                    else
                    {
                        var newMark = _markFactory.CreateMark(userId.Value, courseId.Value, classId.Value, grade); // Sử dụng IMarkFactory
                        TempData["Message"] = newMark != null ? "Mark created successfully." : "Failed to create mark.";
                        TempData["Success"] = newMark != null;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                TempData["Success"] = false;
            }

            return RedirectToAction("ReportMark");
        }
    }
}