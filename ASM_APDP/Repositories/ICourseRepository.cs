﻿using ASM_APDP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Repositories
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAllCourses();
        Course GetCourseById(int id);
        Course GetCourseByName(string courseName);
        bool CreateCourse(Course courseEntity);
        Task<bool> UpdateCourseAsync(Course courseEntity);
        bool DeleteCourse(int id);
    }
}
