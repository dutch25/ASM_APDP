using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public interface IClassRepository
    {
        IEnumerable<Class> GetAll();
        Class GetClassByID(int id);
        Class GetClassByName(string className);
    }
}