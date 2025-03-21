using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_APDP.Models
{
    public class Mark
    {
        [Key]
        public int MarkID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Grade { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
