using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]

        public DateTime? DoB { get; set; }
        public string Email { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]

        public DateTime CreateDate { get; set; }
        public virtual Role Role { get; set; }
    }
}
