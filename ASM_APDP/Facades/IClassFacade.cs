using ASM_APDP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Facades
{
    public interface IClassFacade
    {
        IEnumerable<Class> GetAllClasses();
        Class GetClassById(int id);
        bool CreateClass(Class classEntity);
        Task<bool> UpdateClassAsync(Class classEntity);
        bool DeleteClass(int id);
    }
}
