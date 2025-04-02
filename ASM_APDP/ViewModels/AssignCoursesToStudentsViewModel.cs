namespace ASM_APDP.ViewModels
{
    public class AssignCoursesToStudentsViewModel
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public int? ClassId { get; set; }

        public string ClassName { get; set; }

        public int CourseId { get; set; }

        public List<ASM_APDP.Models.User> Students { get; set; }

        public List<ASM_APDP.Models.Class> Classes { get; set; }

        public List<ASM_APDP.Models.Course> Courses { get; set; }
    }
}