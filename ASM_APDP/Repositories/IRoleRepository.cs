﻿using ASM_APDP.Models;
using System.Numerics;

namespace ASM_APDP.Repositories
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAll();
        Role GetRoleByID(int Id);
        Role GetRoleByName(string RoleName);



    }
}
