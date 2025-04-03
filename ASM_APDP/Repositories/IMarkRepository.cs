using ASM_APDP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Repositories
{
    public interface IMarkRepository
    {
        IEnumerable<Mark> GetAll();
        Task<IEnumerable<Mark>> GetAllAsync(); // Thêm async
        Mark GetMarkByID(int id);
        Mark GetMarkByStudentID(int studentID);
        Mark GetMarkByCourseID(int courseID);
        bool Grade(Mark mark);
        bool Delete(int id);
        bool Update(Mark mark);
        int Create(Mark mark);
    }
}