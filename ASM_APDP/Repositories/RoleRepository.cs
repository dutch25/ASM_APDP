using ASM_APDP.Data;
using ASM_APDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASM_APDP.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DatabaseContext _context;
        public RoleRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetAll()
        {
            try
            {
                return _context.Roles.ToList();
            }
            catch (Exception)
            {
                return new List<Role>();
            }
        }

        public Role GetRoleByID(int Id)
        {
            try
            {
                return _context.Roles.FirstOrDefault(r => r.Id == Id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Role GetRoleByName(string RoleName)
        {
            try
            {
                return _context.Roles.FirstOrDefault(r => r.RoleName == RoleName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
