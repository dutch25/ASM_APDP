using ASM_APDP.Models;
using ASM_APDP.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Facades
{
    public class CourseFacade : ICourseFacade
    {
        private readonly ICourseRepository _courseRepository;

        public CourseFacade(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _courseRepository.GetAllCourses();
        }

        public Course GetCourseById(int id)
        {
            return _courseRepository.GetCourseById(id);
        }

        public Course GetCourseByName(string courseName)
        {
            return _courseRepository.GetCourseByName(courseName);
        }

        public bool CreateCourse(Course courseEntity)
        {
            return _courseRepository.CreateCourse(courseEntity);
        }

        public async Task<bool> UpdateCourseAsync(Course courseEntity)
        {
            return await _courseRepository.UpdateCourseAsync(courseEntity);
        }

        public bool DeleteCourse(int id)
        {
            return _courseRepository.DeleteCourse(id);
        }
    }
}
