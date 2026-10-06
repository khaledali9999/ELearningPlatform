using ELearningPlatform.Repositories;
using ELearningPlatform.Repositories.Interfaces;
using ELearningPlatform.Services;
using ELearningPlatform.Services.Interfaces;

namespace ELearningPlatform.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers every repository and service (Dependency Injection in one place).</summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IInstructorRepository, InstructorRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IInstructorService, InstructorService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<IReviewService, ReviewService>();

        return services;
    }
}
