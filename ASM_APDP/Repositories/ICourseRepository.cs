using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAll();
        Course GetCourseByID(int id);
        Course GetCourseByName(string courseName);
    }
}

