using ASM_APDP.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM_APDP.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Mark> Marks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships with cascading deletes
            modelBuilder.Entity<Mark>()
                .HasOne(m => m.User)
                .WithMany(u => u.Marks)
                .HasForeignKey(m => m.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Mark>()
                .HasOne(m => m.Course)
                .WithMany(c => c.Marks)
                .HasForeignKey(m => m.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Mark>()
                .HasOne(m => m.Class)
                .WithMany(c => c.Marks)
                .HasForeignKey(m => m.ClassID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.User)
                .WithMany(u => u.Classes)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Course)
                .WithMany(c => c.Classes)
                .HasForeignKey(c => c.CourseID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
