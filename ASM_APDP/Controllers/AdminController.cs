using ASM_APDP.Facades;
using ASM_APDP.Models;
using ASM_APDP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
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
            _classFacade = classFacade ?? throw new ArgumentNullException(nameof(classFacade));
            _userFacade = userFacade ?? throw new ArgumentNullException(nameof(userFacade));
            _courseFacade = courseFacade ?? throw new ArgumentNullException(nameof(courseFacade));
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
            var courses = await Task.Run(() => _courseFacade.GetAllCourses()?.ToList() ?? new List<Course>());
            return View(courses);
        }

        // GET: AssignCoursesToStudents
        public IActionResult AssignCoursesToStudents()
        {
            var classes = _classFacade.GetAllClasses()?.ToList() ?? new List<Class>();
            var students = _userFacade.GetAllUsers()?.Where(u => u?.Role?.RoleName == "Student").ToList() ?? new List<User>();
            var courses = _courseFacade.GetAllCourses()?.ToList() ?? new List<Course>();

            var viewModel = new AssignCoursesToStudentsViewModel
            {
                Students = students,
                Classes = classes,
                Courses = courses
            };

            ViewBag.Classes = classes;
            ViewBag.Students = students;
            ViewBag.Courses = courses;
            ViewBag.ClassesJson = System.Text.Json.JsonSerializer.Serialize(classes.Select(c => new { c.ClassID, c.ClassName }));

            Debug.WriteLine($"ClassesJson: {ViewBag.ClassesJson}");
            Debug.WriteLine($"Classes Count: {classes.Count}");

            return View(viewModel);
        }

        // POST: AssignStudentToClass (Add new assignment)
        [HttpPost]
        public IActionResult AssignStudentToClass(AssignCoursesToStudentsViewModel model)
        {
            Debug.WriteLine($"Assign: StudentId: {model.StudentId}, ClassId: {model.ClassId}, ClassName: {model.ClassName}, CourseId: {model.CourseId}");

            // Validate input (ClassId is optional for new assignments)
            if (model.StudentId <= 0 || string.IsNullOrEmpty(model.ClassName) || model.CourseId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid input data. Ensure all required fields are filled. " +
                    $"[StudentId: {model.StudentId}, ClassName: {model.ClassName}, CourseId: {model.CourseId}]";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var student = _userFacade.GetUserById(model.StudentId);
            if (student == null)
            {
                TempData["ErrorMessage"] = $"Student with ID {model.StudentId} not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var course = _courseFacade.GetCourseById(model.CourseId);
            if (course == null)
            {
                TempData["ErrorMessage"] = $"Course with ID {model.CourseId} not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            // Check for duplicate assignment
            var isAlreadyAssigned = _classFacade.GetAllClasses()
                ?.Any(c => c.UserID == model.StudentId && c.ClassName == model.ClassName && c.CourseID == model.CourseId) ?? false;
            if (isAlreadyAssigned)
            {
                TempData["ErrorMessage"] = "Student is already assigned to this class and course.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            // Create new class assignment using ClassName
            var newClass = new Class
            {
                ClassName = model.ClassName,
                UserID = model.StudentId,
                CourseID = model.CourseId
            };

            bool isCreated = _classFacade.CreateClass(newClass);
            if (isCreated)
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
                ClassName = classEntity.ClassName ?? string.Empty,
                CourseId = classEntity.CourseID ?? 0,
                Students = _userFacade.GetAllUsers()?.Where(u => u?.Role?.RoleName == "Student").ToList() ?? new List<User>(),
                Classes = _classFacade.GetAllClasses()?.ToList() ?? new List<Class>(),
                Courses = _courseFacade.GetAllCourses()?.ToList() ?? new List<Course>()
            };

            ViewBag.Classes = model.Classes;
            ViewBag.Students = model.Students;
            ViewBag.Courses = model.Courses;

            return View(model);
        }

        // POST: UpdateStudentInClass
        [HttpPost]
        public IActionResult UpdateStudentInClass(AssignCoursesToStudentsViewModel model)
        {
            System.Diagnostics.Debug.WriteLine($"Update: ClassId: {model.ClassId}, StudentId: {model.StudentId}, CourseId: {model.CourseId}");

            if (model.ClassId <= 0 || model.StudentId <= 0 || model.CourseId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid input data. Ensure all fields are filled.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var classEntity = _classFacade.GetClassById(model.ClassId);
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
                Courses = _courseFacade.GetAllCourses()?.ToList() ?? new List<Course>()
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
            model.Courses = _courseFacade.GetAllCourses()?.ToList() ?? new List<Course>();
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
                CourseName = course.CourseName ?? string.Empty,
                Courses = _courseFacade.GetAllCourses()?.ToList() ?? new List<Course>()
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
            model.Courses = _courseFacade.GetAllCourses()?.ToList() ?? new List<Course>();
            return View("CourseManagement", model);
        }
    }
}