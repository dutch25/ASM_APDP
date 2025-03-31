using ASM_APDP.Facades;
using ASM_APDP.Models;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASM_APDP.Controllers
{
    public class AdminController : Controller
    {
        private readonly IClassFacade _classFacade;
        private readonly IUserFacade _userFacade;
        private readonly ICourseFacade _courseFacade;

        public AdminController(IClassFacade classFacade, IUserFacade userFacade, ICourseFacade courseFacade)
        {
            _classFacade = classFacade;
            _userFacade = userFacade;
            _courseFacade = courseFacade;
        }

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
            var classes = _classFacade.GetAllClasses();
            var students = _userFacade.GetAllUsers();
            var courses = _courseFacade.GetAllCourses();
            ViewBag.Classes = classes;
            ViewBag.Students = students;
            ViewBag.Courses = courses;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignStudentToClass(AssignCoursesToStudentsViewModel model)
        {
            if (model.StudentId <= 0 || string.IsNullOrEmpty(model.ClassName) || model.CourseId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid input data.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var student = _userFacade.GetUserById(model.StudentId);
            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var existingClass = _classFacade.GetClassByName(model.ClassName);
            if (existingClass == null)
            {
                TempData["ErrorMessage"] = "Class not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            existingClass.UserID = model.StudentId;
            existingClass.CourseID = model.CourseId;

            bool isAssigned = await _classFacade.UpdateClassAsync(existingClass);
            if (isAssigned)
            {
                TempData["SuccessMessage"] = "Student assigned successfully.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            TempData["ErrorMessage"] = "Error assigning student to class.";
            return RedirectToAction("AssignCoursesToStudents");
        }
    }
}
