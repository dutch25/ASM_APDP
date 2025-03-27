using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAll();
        Course GetCourseByID(int id);
        Course GetCourseByName(string courseName);
        int Create(Course courseEntity);
        bool Delete(int id);
        bool Update(Course courseEntity);

    }
}

