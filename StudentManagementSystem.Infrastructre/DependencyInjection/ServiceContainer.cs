using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Application.Interfaces.Authentication;
using StudentManagementSystem.Domain.Core.AuthenticationEntities;
using StudentManagementSystem.Domain.Core.Interfaces;
using StudentManagementSystem.Infrastructre.Data;
using StudentManagementSystem.Infrastructre.Repositories;
using StudentManagementSystem.Infrastructre.Repositories.Authentications;

namespace StudentManagementSystem.Infrastructre.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructreService(
       this IServiceCollection services,
       IConfiguration _config)
    {

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<IAccount, Account>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
        });

        // Update this part to properly configure Identity
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        services.AddAuthentication();

        services.AddAuthorization();
        return services;
    }
}
