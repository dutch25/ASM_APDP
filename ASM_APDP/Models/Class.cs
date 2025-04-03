using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_APDP.Models
{
    public class Class
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        [StringLength(100)]
        public string ClassName { get; set; }

        // Cho phép UserID nullable để tránh lỗi khi User bị xóa
        public int? UserID { get; set; }
        public int? CourseID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    }
}
