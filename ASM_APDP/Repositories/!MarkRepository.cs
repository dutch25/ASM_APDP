using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface IMarkRepository
    {
        IEnumerable<Mark> GetAll();
        Mark GetMarkByID(int id);
        Mark GetMarkByStudentID(int studentID);
        Mark GetMarkByCourseID(int courseID);
        int Grade(Mark mark);
        int Delete(int id);
        int Update(Mark mark);
        int Create(Mark mark);
    }
}