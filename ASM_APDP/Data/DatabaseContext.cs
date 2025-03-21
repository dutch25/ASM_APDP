using ASM_APDP.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM_APDP.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base (options)
        {

        }
        public DbSet<Models.Role> Roles { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Mark> Marks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Specify the column type for the Grade property
            modelBuilder.Entity<Mark>()
                .Property(m => m.Grade)
                .HasColumnType("decimal(5, 2)");
        }
    }
}
