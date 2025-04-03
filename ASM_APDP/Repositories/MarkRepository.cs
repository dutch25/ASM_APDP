using ASM_APDP.Data;
using ASM_APDP.Facades;
using ASM_APDP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_APDP.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        private readonly DatabaseContext _context;

        public MarkRepository(DatabaseContext context)
        {
            _context = context;
        }
        public int Create(Mark mark)
        {
            try
            {
                _context.Marks.Add(mark);
                return _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Log the detailed error (you can use a logging framework instead of Debug)
                Debug.WriteLine($"Error creating mark: {ex.InnerException?.Message ?? ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
                return 0;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var mark = _context.Marks.Find(id);
                if (mark == null)
                {
                    return false;
                }
                _context.Marks.Remove(mark);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Mark> GetAll()
        {
            try
            {
                return _context.Marks
                    .Include(m => m.User)   
                    .Include(m => m.Course)  
                    .Include(m => m.Class)  
                    .ToList();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Mark>();
            }
        }

        public async Task<IEnumerable<Mark>> GetAllAsync()
        {
            try
            {
                return await _context.Marks
                    .Include(m => m.User)
                    .Include(m => m.Course)
                    .Include(m => m.Class)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Mark>();
            }
        }

        public Mark GetMarkByID(int id)
        {
            try
            {
                return _context.Marks
                    .Include(m => m.User)
                    .Include(m => m.Course)
                    .Include(m => m.Class)
                    .FirstOrDefault(m => m.MarkID == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Mark GetMarkByStudentID(int studentID)
        {
            try
            {
                return _context.Marks
                    .Include(m => m.User)
                    .Include(m => m.Course)
                    .Include(m => m.Class)
                    .FirstOrDefault(m => m.UserID == studentID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Mark GetMarkByCourseID(int courseID)
        {
            try
            {
                return _context.Marks
                    .Include(m => m.User)
                    .Include(m => m.Course)
                    .Include(m => m.Class)
                    .FirstOrDefault(m => m.CourseID == courseID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Grade(Mark mark)
        {
            try
            {
                _context.Marks.Update(mark);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Mark mark)
        {
            try
            {
                _context.Marks.Update(mark);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}