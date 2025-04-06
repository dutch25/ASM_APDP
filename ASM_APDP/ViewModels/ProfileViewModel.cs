namespace ASM_APDP.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public DateTime? DoB { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }
    }
}

