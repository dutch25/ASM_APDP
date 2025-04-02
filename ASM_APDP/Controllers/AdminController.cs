using ASM_APDP.Facades;
using ASM_APDP.Models;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

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

        public IActionResult ViewAllAssignCourses()
        {
            return View();
        }

        public async Task<IActionResult> ViewAllCourses()
        {
            var courses = await Task.Run(() => _courseFacade.GetAllCourses().ToList());
            return View(courses);
        }
        // GET: AssignCoursesToStudents
        public IActionResult AssignCoursesToStudents()
        {
            var classes = _classFacade.GetAllClasses().ToList();
            var students = _userFacade.GetAllUsers().ToList();
            var courses = _courseFacade.GetAllCourses().ToList();
            ViewBag.Classes = classes;
            ViewBag.Students = students;
            ViewBag.Courses = courses;
            return View(new AssignCoursesToStudentsViewModel());
        }

        // POST: AssignStudentToClass (Add new assignment)
        [HttpPost]
        public IActionResult AssignStudentToClass(AssignCoursesToStudentsViewModel model)
        {
            // Log for debugging
            System.Diagnostics.Debug.WriteLine($"Assign: StudentId: {model.StudentId}, ClassName: {model.ClassName}, CourseId: {model.CourseId}");

            if (model.StudentId <= 0 || string.IsNullOrEmpty(model.ClassName) || model.CourseId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid input data. Ensure all fields are filled.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var student = _userFacade.GetUserById(model.StudentId);
            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var course = _courseFacade.GetCourseById(model.CourseId);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            // Check for duplicate assignment
            var isAlreadyAssigned = _classFacade.GetAllClasses()
                .Any(c => c.UserID == model.StudentId && c.CourseID == model.CourseId && c.ClassName == model.ClassName);

            if (isAlreadyAssigned)
            {
                TempData["ErrorMessage"] = "Student is already assigned to this class and course.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            // Create new class assignment (no need for existing class check)
            var newClass = new Class
            {
                ClassName = model.ClassName,
                UserID = model.StudentId,
                CourseID = model.CourseId
            };

            bool isAssigned = _classFacade.CreateClass(newClass);
            if (isAssigned)
            {
                TempData["SuccessMessage"] = "Student assigned successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error assigning student to class.";
            }

            return RedirectToAction("AssignCoursesToStudents");
        }

        // POST: DeleteStudentFromClass
        [HttpPost]
        public IActionResult DeleteStudentFromClass(int classId)
        {
            var classEntity = _classFacade.GetClassById(classId);
            if (classEntity != null)
            {
                var isDeleted = _classFacade.DeleteClass(classId);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "Student removed from class successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error removing student from class.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Class not found.";
            }
            return RedirectToAction("AssignCoursesToStudents");
        }

        // GET: UpdateStudentInClass
        [HttpGet]
        public IActionResult UpdateStudentInClass(int classId)
        {
            var classEntity = _classFacade.GetClassById(classId);
            if (classEntity == null)
            {
                TempData["ErrorMessage"] = "Class not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var model = new AssignCoursesToStudentsViewModel
            {
                ClassId = classId,
                StudentId = classEntity.UserID ?? 0,
                ClassName = classEntity.ClassName,
                CourseId = classEntity.CourseID ?? 0
            };
            ViewBag.Classes = _classFacade.GetAllClasses().ToList();
            ViewBag.Students = _userFacade.GetAllUsers().ToList();
            ViewBag.Courses = _courseFacade.GetAllCourses().ToList();
            return View(model);
        }

        // POST: UpdateStudentInClass
        [HttpPost]
        public IActionResult UpdateStudentInClass(AssignCoursesToStudentsViewModel model)
        {
            // Log for debugging
            System.Diagnostics.Debug.WriteLine($"Update: ClassId: {model.ClassId}, StudentId: {model.StudentId}, ClassName: {model.ClassName}, CourseId: {model.CourseId}");

            if (model.ClassId == null || model.StudentId <= 0 || string.IsNullOrEmpty(model.ClassName) || model.CourseId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid input data. Ensure all fields are filled.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var classEntity = _classFacade.GetClassById(model.ClassId.Value);
            if (classEntity == null)
            {
                TempData["ErrorMessage"] = "Class not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var student = _userFacade.GetUserById(model.StudentId);
            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var course = _courseFacade.GetCourseById(model.CourseId);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            classEntity.UserID = model.StudentId;
            classEntity.ClassName = model.ClassName;
            classEntity.CourseID = model.CourseId;

            var isUpdated = _classFacade.UpdateClassAsync(classEntity).Result;
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Student updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating student.";
            }

            return RedirectToAction("AssignCoursesToStudents");
        }
        public IActionResult CourseManagement()
        {
            var model = new AddCourseViewModel
            {
                Courses = _courseFacade.GetAllCourses().ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            return View(new AddCourseViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var course = new Course
                {
                    CourseName = model.CourseName
                };

                var isCreated = _courseFacade.CreateCourse(course);
                if (isCreated)
                {
                    TempData["SuccessMessage"] = "Course added successfully.";
                    return RedirectToAction("CourseManagement");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error adding course.";
                }
            }
            model.Courses = _courseFacade.GetAllCourses().ToList();
            return View("CourseManagement", model);
        }

        [HttpPost]
        public IActionResult DeleteCourse(int courseId)
        {
            var course = _courseFacade.GetCourseById(courseId);
            if (course != null)
            {
                var isDeleted = _courseFacade.DeleteCourse(courseId);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "Course deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting course.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Course not found.";
            }
            return RedirectToAction("CourseManagement");
        }

        [HttpGet]
        public IActionResult UpdateCourse(int courseId)
        {
            var course = _courseFacade.GetCourseById(courseId);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("CourseManagement");
            }

            var model = new AddCourseViewModel
            {
                CourseId = course.CourseID,
                CourseName = course.CourseName,
                Courses = _courseFacade.GetAllCourses().ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourse(AddCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var course = _courseFacade.GetCourseById(model.CourseId);
                if (course == null)
                {
                    TempData["ErrorMessage"] = "Course not found.";
                    return RedirectToAction("CourseManagement");
                }

                course.CourseName = model.CourseName;
                var isUpdated = await _courseFacade.UpdateCourseAsync(course);
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Course updated successfully.";
                    return RedirectToAction("CourseManagement");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error updating course.";
                }
            }
            model.Courses = _courseFacade.GetAllCourses().ToList();
            return View("CourseManagement", model);
        }
    }
}
