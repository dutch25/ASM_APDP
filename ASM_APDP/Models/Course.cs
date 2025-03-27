using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_APDP.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; }

        // Thêm danh sách Class và Marks để thiết lập quan hệ 1-N
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}
