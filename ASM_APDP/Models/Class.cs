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

        [ForeignKey("User")]
        public int? UserID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
