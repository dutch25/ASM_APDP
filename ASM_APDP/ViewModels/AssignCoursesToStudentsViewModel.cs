using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASM_APDP.Models;

namespace ASM_APDP.ViewModels
{
    public class AssignCoursesToStudentsViewModel
    {
        [Display(Name = "Học sinh")]
        [Required(ErrorMessage = "Vui lòng chọn học sinh")]
        public int StudentId { get; set; }

        public string StudentName { get; set; } // Để hiển thị thông tin (không cần nhập)

        [Display(Name = "Lớp học")]
        [Required(ErrorMessage = "Vui lòng chọn lớp học")]
        public int ClassId { get; set; } // Sử dụng ClassId để chọn lớp hiện có

        public string ClassName { get; set; } // Để hiển thị thông tin (không cần nhập)

        [Display(Name = "Khóa học")]
        [Required(ErrorMessage = "Vui lòng chọn khóa học")]
        public int CourseId { get; set; }

        // Danh sách để hiển thị trong dropdown
        public List<User> Students { get; set; } = new List<User>();
        public List<Class> Classes { get; set; } = new List<Class>();
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}