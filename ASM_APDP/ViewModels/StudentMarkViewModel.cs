namespace ASM_APDP.ViewModels
{
    public class StudentMarkViewModel
    {
        public int UserId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public string ClassName { get; set; }
        public int? MarkId { get; set; }
        public decimal? Grade { get; set; } // Đảm bảo là decimal? để hỗ trợ null
        public int CourseId { get; set; }
        public int ClassId { get; set; }
    }
}