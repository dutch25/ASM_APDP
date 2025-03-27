using ASM_APDP.Data;
using ASM_APDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASM_APDP.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DatabaseContext _context;

        public CourseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int Create(Course courseEntity)
        {
            try
            {
                _context.Courses.Add(courseEntity);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var courseEntity = _context.Courses.Find(id);
                if (courseEntity == null)
                {
                    return false;
                }
                _context.Courses.Remove(courseEntity);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Course> GetAll()
        {
            try
            {
                return _context.Courses.ToList();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Course>();
            }
        }

        public Course GetCourseByID(int id)
        {
            try
            {
                return _context.Courses.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Course GetCourseByName(string courseName)
        {
            try
            {
                return _context.Courses.FirstOrDefault(c => c.CourseName == courseName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Update(Course courseEntity)
        {
            try
            {
                _context.Courses.Update(courseEntity);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}