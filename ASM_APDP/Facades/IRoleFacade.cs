// IRoleFacade.cs
using ASM_APDP.Models;

namespace ASM_APDP.Facades
{
    public interface IRoleFacade
    {
        IEnumerable<Role> GetAllRoles();
        Role GetRoleById(int Id);
        Role GetRoleByName(string RoleName);
    }
}
