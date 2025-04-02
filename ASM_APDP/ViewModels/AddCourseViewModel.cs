using ASM_APDP.Models;

namespace ASM_APDP.ViewModels
{
    public class AddCourseViewModel
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();

    }
}
