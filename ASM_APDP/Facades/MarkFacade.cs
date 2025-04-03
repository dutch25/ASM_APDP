using ASM_APDP.Models;
using ASM_APDP.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Facades
{
    public class MarkFacade : IMarkFacade
    {
        private readonly IMarkRepository _markRepository;

        public MarkFacade(IMarkRepository markRepository)
        {
            _markRepository = markRepository;
        }

        public async Task<List<Mark>> GetAllMarksAsync()
        {
            return await Task.Run(() => _markRepository.GetAll().ToList());
        }

        public Mark GetMarkById(int id)
        {
            return _markRepository.GetMarkByID(id);
        }

        public Mark GetMarkByStudentId(int studentId)
        {
            return _markRepository.GetMarkByStudentID(studentId);
        }

        public Mark GetMarkByCourseId(int courseId)
        {
            return _markRepository.GetMarkByCourseID(courseId);
        }

        public bool GradeMark(Mark mark)
        {
            return _markRepository.Grade(mark);
        }

        public bool DeleteMark(int id)
        {
            return _markRepository.Delete(id);
        }

        public bool UpdateMark(Mark mark)
        {
            return _markRepository.Update(mark);
        }

        public int CreateMark(Mark mark)
        {
            return _markRepository.Create(mark);
        }
    }
}
