﻿using System.ComponentModel.DataAnnotations;

namespace ASM_APDP.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public int RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
