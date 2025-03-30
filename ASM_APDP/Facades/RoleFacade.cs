// RoleFacade.cs
using ASM_APDP.Models;
using ASM_APDP.Repositories;

namespace ASM_APDP.Facades
{
    public class RoleFacade : IRoleFacade
    {
        private readonly IRoleRepository _roleRepository;

        public RoleFacade(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _roleRepository.GetAll();
        }

        public Role GetRoleById(int Id)
        {
            return _roleRepository.GetRoleByID(Id);
        }

        public Role GetRoleByName(string RoleName)
        {
            return _roleRepository.GetRoleByName(RoleName);
        }
    }
}
