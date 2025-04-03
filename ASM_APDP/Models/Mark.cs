using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_APDP.Models
{
    public class Mark
    {
        [Key]
        public int MarkID { get; set; }

        public int UserID { get; set; }
        public int CourseID { get; set; }

        public int ClassID { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Grade { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("ClassID")]
        public virtual Class Class { get; set; }
    }
}
