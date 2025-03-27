using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_APDP.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? DoB { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}
