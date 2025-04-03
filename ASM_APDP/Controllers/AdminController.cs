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
        private readonly IMarkFacade _markFacade;

        public AdminController(IClassFacade classFacade, IUserFacade userFacade, ICourseFacade courseFacade, IMarkFacade markFacade)
        {
            _classFacade = classFacade ?? throw new ArgumentNullException(nameof(classFacade));
            _userFacade = userFacade ?? throw new ArgumentNullException(nameof(userFacade));
            _courseFacade = courseFacade ?? throw new ArgumentNullException(nameof(courseFacade));
            _markFacade = markFacade ?? throw new ArgumentNullException(nameof(markFacade));
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }

        // GET: CreateClass
        public IActionResult CreateClass()
        {
            var classes = _classFacade.GetAllClasses()?.ToList() ?? new List<Class>();
            // Filter unique classes by ClassName
            var uniqueClasses = classes
                .GroupBy(c => c.ClassName)
                .Select(g => g.First())
                .ToList();

            var viewModel = new CreateClassViewModel
            {
                Classes = uniqueClasses // Use unique classes for display
            };
            return View(viewModel);
        }

        // POST: AddClass
        [HttpPost]
        public IActionResult AddClass(CreateClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if a class with the same name already exists
                var existingClass = _classFacade.GetAllClasses()
                    ?.FirstOrDefault(c => c.ClassName.Equals(model.ClassName, StringComparison.OrdinalIgnoreCase));
                if (existingClass != null)
                {
                    TempData["ErrorMessage"] = "A class with this name already exists.";
                    model.Classes = _classFacade.GetAllClasses()
                        ?.GroupBy(c => c.ClassName)
                        .Select(g => g.First())
                        .ToList() ?? new List<Class>();
                    return View("CreateClass", model);
                }

                var newClass = new Class
                {
                    ClassName = model.ClassName
                };
                bool isCreated = _classFacade.CreateClass(newClass);
                if (isCreated)
                {
                    TempData["SuccessMessage"] = "Class created successfully.";
                    return RedirectToAction("CreateClass");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error creating class.";
                }
            }

            model.Classes = _classFacade.GetAllClasses()
                ?.GroupBy(c => c.ClassName)
                .Select(g => g.First())
                .ToList() ?? new List<Class>();
            return View("CreateClass", model);
        }

        // POST: UpdateClass
        [HttpPost]
        public IActionResult UpdateClass(CreateClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingClass = _classFacade.GetClassById(model.ClassId);
                if (existingClass == null)
                {
                    TempData["ErrorMessage"] = "Class not found.";
                    return RedirectToAction("CreateClass");
                }

                // Check if the new name conflicts with another existing class
                var duplicateClass = _classFacade.GetAllClasses()
                    ?.FirstOrDefault(c => c.ClassName.Equals(model.ClassName, StringComparison.OrdinalIgnoreCase) && c.ClassID != model.ClassId);
                if (duplicateClass != null)
                {
                    TempData["ErrorMessage"] = "Another class with this name already exists.";
                    model.Classes = _classFacade.GetAllClasses()
                        ?.GroupBy(c => c.ClassName)
                        .Select(g => g.First())
                        .ToList() ?? new List<Class>();
                    return View("CreateClass", model);
                }

                existingClass.ClassName = model.ClassName;
                var isUpdated = _classFacade.UpdateClassAsync(existingClass).Result;
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = ".idxClass updated successfully.";
                    return RedirectToAction("CreateClass");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error updating class.";
                }
            }

            model.Classes = _classFacade.GetAllClasses()
                ?.GroupBy(c => c.ClassName)
                .Select(g => g.First())
                .ToList() ?? new List<Class>();
            return View("CreateClass", model);
        }

        // POST: DeleteClass
        [HttpPost]
        public IActionResult DeleteClass(int classId)
        {
            var classEntity = _classFacade.GetClassById(classId);
            if (classEntity != null)
            {
                var isDeleted = _classFacade.DeleteClass(classId);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "Class deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting class.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Class not found.";
            }
            return RedirectToAction("CreateClass");
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

            // Filter unique classes by ClassName
            var uniqueClasses = classes
                .GroupBy(c => c.ClassName)
                .Select(g => g.First())
                .ToList();

            var viewModel = new AssignCoursesToStudentsViewModel
            {
                Students = students,
                Classes = uniqueClasses, // Use unique classes for the form
                Courses = courses
            };

            ViewBag.Classes = classes; // Full list for table display
            ViewBag.UniqueClasses = uniqueClasses; // Unique list for dropdowns
            ViewBag.Students = students;
            ViewBag.Courses = courses;
            ViewBag.ClassesJson = System.Text.Json.JsonSerializer.Serialize(uniqueClasses.Select(c => new { c.ClassID, c.ClassName }));

            Debug.WriteLine($"Classes Count: {classes.Count}");
            Debug.WriteLine($"Unique Classes Count: {uniqueClasses.Count}");
            return View(viewModel);
        }

        // POST: AssignStudentToClass
        [HttpPost]
        public IActionResult AssignStudentToClass(AssignCoursesToStudentsViewModel model)
        {
            Debug.WriteLine($"Assign: StudentId: {model.StudentId}, ClassId: {model.ClassId}, CourseId: {model.CourseId}");

            if (model.StudentId <= 0 || model.ClassId <= 0 || model.CourseId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid input data. Ensure all fields are filled.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var student = _userFacade.GetUserById(model.StudentId);
            var course = _courseFacade.GetCourseById(model.CourseId);
            var selectedClass = _classFacade.GetClassById(model.ClassId);

            if (student == null || course == null || selectedClass == null)
            {
                TempData["ErrorMessage"] = "Student, course, or class not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var isAlreadyAssigned = _classFacade.GetAllClasses()
                ?.Any(c => c.UserID == model.StudentId && c.ClassID == model.ClassId && c.CourseID == model.CourseId) ?? false;
            if (isAlreadyAssigned)
            {
                TempData["ErrorMessage"] = "Student is already assigned to this class and course.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var newClass = new Class
            {
                ClassName = selectedClass.ClassName,
                UserID = model.StudentId,
                CourseID = model.CourseId
            };

            bool isCreated = _classFacade.CreateClass(newClass);
            if (isCreated)
            {
                // Lấy ClassID của bản ghi vừa tạo (giả sử CreateClass trả về ClassID hoặc bạn cần truy vấn lại)
                var createdClass = _classFacade.GetAllClasses()
                    ?.FirstOrDefault(c => c.UserID == model.StudentId && c.CourseID == model.CourseId && c.ClassName == selectedClass.ClassName);

                if (createdClass != null)
                {
                    // Tạo bản ghi Mark mặc định
                    var newMark = new Mark
                    {
                        UserID = model.StudentId,
                        CourseID = model.CourseId,
                        ClassID = createdClass.ClassID,
                        Grade = null // Điểm mặc định là null
                    };
                    _markFacade.CreateMark(newMark); // Gọi facade để tạo Mark
                }

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

            var classes = _classFacade.GetAllClasses()?.ToList() ?? new List<Class>();
            var uniqueClasses = classes
                .GroupBy(c => c.ClassName)
                .Select(g => g.First())
                .ToList();

            var model = new AssignCoursesToStudentsViewModel
            {
                ClassId = classId,
                StudentId = classEntity.UserID ?? 0,
                ClassName = classEntity.ClassName ?? string.Empty,
                CourseId = classEntity.CourseID ?? 0,
                Students = _userFacade.GetAllUsers()?.Where(u => u?.Role?.RoleName == "Student").ToList() ?? new List<User>(),
                Classes = uniqueClasses, // Use unique classes for dropdown
                Courses = _courseFacade.GetAllCourses()?.ToList() ?? new List<Course>()
            };

            ViewBag.Classes = classes; // Full list for table
            ViewBag.Students = model.Students;
            ViewBag.Courses = model.Courses;

            return View(model);
        }

        // POST: UpdateStudentInClass
        [HttpPost]
        public IActionResult UpdateStudentInClass(AssignCoursesToStudentsViewModel model, int NewClassId)
        {
            Debug.WriteLine($"Update: Original ClassId: {model.ClassId}, NewClassId: {NewClassId}, StudentId: {model.StudentId}, CourseId: {model.CourseId}");

            if (model.ClassId <= 0 || model.StudentId <= 0 || model.CourseId <= 0 || NewClassId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid input data. Ensure all fields are filled.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var classEntity = _classFacade.GetClassById(model.ClassId); // Original class to update
            if (classEntity == null)
            {
                TempData["ErrorMessage"] = "Original class not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            var student = _userFacade.GetUserById(model.StudentId);
            var course = _courseFacade.GetCourseById(model.CourseId);
            var newClass = _classFacade.GetClassById(NewClassId); // New class selected from dropdown

            if (student == null || course == null || newClass == null)
            {
                TempData["ErrorMessage"] = "Student, course, or new class not found.";
                return RedirectToAction("AssignCoursesToStudents");
            }

            // Update the entity with new values
            classEntity.UserID = model.StudentId;
            classEntity.CourseID = model.CourseId;
            classEntity.ClassName = newClass.ClassName; // Update ClassName based on NewClassId

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