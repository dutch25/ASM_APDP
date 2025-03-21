using ASM_APDP.Data;
using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        private readonly DatabaseContext _context;

        public MarkRepository(DatabaseContext context)
        {
            _context = context;
        }

        int IMarkRepository.Create(Mark mark)
        {
            try
            {
                _context.Marks.Add(mark);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        bool IMarkRepository.Delete(int id)
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

        IEnumerable<Mark> IMarkRepository.GetAll()
        {
            try
            {
                return _context.Marks.ToList();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Mark>();
            }
        }

        Mark IMarkRepository.GetMarkByID(int id)
        {
            try
            {
                return _context.Marks.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        Mark IMarkRepository.GetMarkByStudentID(int studentID)
        {
            try
            {
                return _context.Marks.FirstOrDefault(m => m.UserID == studentID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        Mark IMarkRepository.GetMarkByCourseID(int courseID)
        {
            try
            {
                return _context.Marks.FirstOrDefault(m => m.CourseID == courseID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        bool IMarkRepository.Grade(Mark mark)
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

        bool IMarkRepository.Update(Mark mark)
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