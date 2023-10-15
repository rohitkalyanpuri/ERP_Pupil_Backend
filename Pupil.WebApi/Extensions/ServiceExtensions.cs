using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pupil.DataLayer;
using Pupil.Services;

namespace Pupil.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
           => services.AddCors(options =>
           {
               options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
           });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });
        //public static void ConfigureLoggerService(this IServiceCollection services)
        //    => services.AddScoped<ILoggerManager, LoggerManager>();
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<DBContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(ParentService), typeof(ParentService));
            services.AddTransient(typeof(PupilAuthenticationService), typeof(PupilAuthenticationService));
            services.AddTransient(typeof(GradeService), typeof(GradeService));
            services.AddTransient(typeof(AcademicYearService), typeof(AcademicYearService));
            services.AddTransient(typeof(DivisionService), typeof(DivisionService));
            services.AddTransient(typeof(EnrollmentService), typeof(EnrollmentService));
            services.AddTransient(typeof(ExamTypeService), typeof(ExamTypeService));
            services.AddTransient(typeof(StudentService), typeof(StudentService));
            services.AddTransient(typeof(CourseService), typeof(CourseService));
            services.AddTransient(typeof(FeeTypeService), typeof(FeeTypeService));
            services.AddTransient(typeof(FeeStructureService), typeof(FeeStructureService));

            return services;
        }
    }
}
