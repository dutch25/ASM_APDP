﻿using Microsoft.EntityFrameworkCore;

namespace ASM_APDP.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base (options)
        {

        }
        public DbSet<Models.Role> Roles { get; set; }
        public DbSet<Models.User> Users { get; set; }
    }
}
