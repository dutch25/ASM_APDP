using ASM_APDP.Models;

namespace ASM_APDP.ViewModels
{
    public class CreateClassViewModel
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public List<Class> Classes { get; set; } = new List<Class>();
    }
}
