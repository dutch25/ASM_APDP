using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ASM_APDP.Data;
using ASM_APDP.Repositories;
using ASM_APDP.Facades;
using ASM_APDP.Factories;
// Thêm namespace chứa IUserRepository

namespace ASM_APDP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Đăng ký DatabaseContext
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FinalQLVSContext")
                ?? throw new InvalidOperationException("Connection string 'FinalQLVSContext' not found.")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            builder.Services.AddScoped<IClassFacade, ClassFacade>();
            builder.Services.AddScoped<ICourseFacade, CourseFacade>();

            builder.Services.AddScoped<IMarkFactory, MarkConcreteFactory>();



            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
