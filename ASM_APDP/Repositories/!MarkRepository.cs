using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface IMarkRepository
    {
        IEnumerable<Mark> GetAll();
        Mark GetMarkByID(int id);
        Mark GetMarkByStudentID(int studentID);
        Mark GetMarkByCourseID(int courseID);
<<<<<<< HEAD
        bool Grade(Mark mark);
        bool Delete(int id);
        bool Update(Mark mark);
=======
        int Grade(Mark mark);
        int Delete(int id);
        int Update(Mark mark);
>>>>>>> ecc12143217cf909f84561c428e053417049b324
        int Create(Mark mark);
    }
}