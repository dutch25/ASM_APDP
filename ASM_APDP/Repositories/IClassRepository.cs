using ASM_APDP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_APDP.Repositories
{
    public interface IClassRepository
    {
        IEnumerable<Class> GetAllClasses();
        Class GetClassById(int id);
        Class GetClassByName(string className);
        bool CreateClass(Class classEntity);
        Task<bool> UpdateClassAsync(Class classEntity);
        bool DeleteClass(int id);
    }
}