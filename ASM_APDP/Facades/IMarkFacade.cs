using ASM_APDP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Facades
{
    public interface IMarkFacade
    {
        Task<List<Mark>> GetAllMarksAsync();
        Mark GetMarkById(int id);
        Mark GetMarkByStudentId(int studentId);
        Mark GetMarkByCourseId(int courseId);
        bool GradeMark(Mark mark);
        bool DeleteMark(int id);
        bool UpdateMark(Mark mark);
        int CreateMark(Mark mark);
    }
}
