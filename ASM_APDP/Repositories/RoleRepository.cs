using ASM_APDP.Data;
using ASM_APDP.Models;

namespace ASM_APDP.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DatabaseContext _context;
        public RoleRepository(DatabaseContext context)
        {
            _context = context;
        }

        IEnumerable<Role> IRoleRepository.GetAll()
        {
            try
            {
                return _context.Roles.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        Role IRoleRepository.GetRoleByID(int id)
        {
            return null;
        }
        Role IRoleRepository.GetRoleByName(string rolename)
        {
            return null;
        }
        
    }
}
