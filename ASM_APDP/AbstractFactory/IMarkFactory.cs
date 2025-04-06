using ASM_APDP.Models;

namespace ASM_APDP.Factories
{
    public interface IMarkFactory
    {
        Mark CreateMark(int userId, int courseId, int classId, decimal? grade = null);
        Mark GetMarkById(int markId);
        IEnumerable<Mark> GetAllMarks();
        bool UpdateMark(Mark mark);
        bool DeleteMark(int markId);
    }
}