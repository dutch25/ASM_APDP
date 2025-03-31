using ASM_APDP.Models;
using ASM_APDP.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Facades
{
    public class ClassFacade : IClassFacade
    {
        private readonly IClassRepository _classRepository;

        public ClassFacade(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public IEnumerable<Class> GetAllClasses()
        {
            return _classRepository.GetAllClasses();
        }

        public Class GetClassById(int id)
        {
            return _classRepository.GetClassById(id);
        }

        public Class GetClassByName(string className)
        {
            return _classRepository.GetClassByName(className);
        }

        public bool CreateClass(Class classEntity)
        {
            return _classRepository.CreateClass(classEntity);
        }

        public async Task<bool> UpdateClassAsync(Class classEntity)
        {
            return await _classRepository.UpdateClassAsync(classEntity);
        }

        public bool DeleteClass(int id)
        {
            return _classRepository.DeleteClass(id);
        }
    }
}

