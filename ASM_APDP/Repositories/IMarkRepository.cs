using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface IMarkRepository
    {
        IEnumerable<Mark> GetAll();
        Mark GetMarkByID(int id);
        Mark GetMarkByStudentID(int studentID);
        Mark GetMarkByCourseID(int courseID);
        bool Grade(Mark mark);
        bool Delete(int id);
        bool Update(Mark mark);
        int Create(Mark mark);
    }
}