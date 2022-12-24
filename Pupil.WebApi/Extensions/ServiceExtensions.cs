using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Pupil.Core.Interfaces;
using Pupil.Infrastructure.Services;
using Pupil.WebApi.Filters;

namespace Pupil.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
           => services.AddCors(options =>
           {
               options.AddPolicy("CorsPolicy", builder =>
               builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
           });
        //Swagger
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s => {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Code Pupil API", Version = "v1" });
                s.OperationFilter<SwaggerFilter>();
            });
        }
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IExamTypeService, ExamTypeService>();
            services.AddTransient<IParentService, ParentService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IGradeService, GradeService>();
            services.AddTransient<IDivisionService, DivisionService>();
            services.AddTransient<IAcademicYearService, AcademicYearService>();
            return services;
        }
    }
}
