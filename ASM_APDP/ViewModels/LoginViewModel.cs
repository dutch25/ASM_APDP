using System.ComponentModel.DataAnnotations;

namespace ASM_APDP.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime? DoB { get; set; }

        public int Id { get; set; }
    }
}
