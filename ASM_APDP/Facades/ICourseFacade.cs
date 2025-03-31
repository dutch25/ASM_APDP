using ASM_APDP.Models;
using System.Collections.Generic;

namespace ASM_APDP.Facades
{
    public interface ICourseFacade
    {
        IEnumerable<Course> GetAllCourses();
        Course GetCourseById(int id);
        Course GetCourseByName(string courseName);
        bool CreateCourse(Course courseEntity);
        Task<bool> UpdateCourseAsync(Course courseEntity);
        bool DeleteCourse(int id);
    }
}
