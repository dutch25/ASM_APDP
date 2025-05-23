﻿using ASM_APDP.Data;
using ASM_APDP.Models;
using ASM_APDP.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Facades
{
    public class CourseFacade : ICourseFacade
    {
        private readonly DatabaseContext _context;

        public CourseFacade(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        public Course GetCourseById(int id)
        {
            return _context.Courses.Find(id);
        }

        public Course GetCourseByName(string courseName)
        {
            return _context.Courses.FirstOrDefault(c => c.CourseName == courseName);
        }

        public bool CreateCourse(Course courseEntity)
        {
            _context.Courses.Add(courseEntity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateCourseAsync(Course courseEntity)
        {
            _context.Courses.Update(courseEntity);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool DeleteCourse(int id)
        {
            var courseEntity = _context.Courses
                .Include(c => c.Classes)         // Tải các Class liên quan
                .ThenInclude(cl => cl.Marks)     // Tải các Mark liên quan
                .FirstOrDefault(c => c.CourseID == id);

            if (courseEntity == null) return false;

            _context.Courses.Remove(courseEntity);
            return _context.SaveChanges() > 0;
        }
    }
}

