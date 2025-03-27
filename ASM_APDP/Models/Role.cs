using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_APDP.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string RoleName { get; set; }  // Thêm RoleName để phân biệt các vai trò

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
