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

<<<<<<< HEAD
        bool IMarkRepository.Delete(int id)
=======
        int IMarkRepository.Delete(int id)
>>>>>>> ecc12143217cf909f84561c428e053417049b324
        {
            try
            {
                var mark = _context.Marks.Find(id);
                if (mark == null)
                {
<<<<<<< HEAD
                    return false;
                }
                _context.Marks.Remove(mark);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
=======
                    return 0;
                }
                _context.Marks.Remove(mark);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
>>>>>>> ecc12143217cf909f84561c428e053417049b324
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

<<<<<<< HEAD
        bool IMarkRepository.Grade(Mark mark)
=======
        int IMarkRepository.Grade(Mark mark)
>>>>>>> ecc12143217cf909f84561c428e053417049b324
        {
            try
            {
                _context.Marks.Update(mark);
<<<<<<< HEAD
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
=======
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
>>>>>>> ecc12143217cf909f84561c428e053417049b324
            }
        }
    }
}