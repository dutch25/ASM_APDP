using ASM_APDP.Data;
using ASM_APDP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASM_APDP.Factories
{
    public class MarkConcreteFactory : IMarkFactory
    {
        private readonly DatabaseContext _context;

        public MarkConcreteFactory(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Mark CreateMark(int userId, int courseId, int classId, decimal? grade = null)
        {
            var mark = new Mark
            {
                UserID = userId,
                CourseID = courseId,
                ClassID = classId,
                Grade = grade
            };
            _context.Marks.Add(mark);
            _context.SaveChanges();
            return mark;
        }

        public Mark GetMarkById(int markId)
        {
            return _context.Marks
                .Include(m => m.User)
                .Include(m => m.Course)
                .Include(m => m.Class)
                .FirstOrDefault(m => m.MarkID == markId);
        }

        public IEnumerable<Mark> GetAllMarks()
        {
            return _context.Marks
                .Include(m => m.User)
                .Include(m => m.Course)
                .Include(m => m.Class)
                .ToList();
        }

        public bool UpdateMark(Mark mark)
        {
            _context.Marks.Update(mark);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteMark(int markId)
        {
            var mark = _context.Marks.Find(markId);
            if (mark == null) return false;
            _context.Marks.Remove(mark);
            return _context.SaveChanges() > 0;
        }
    }
}