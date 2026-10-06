using ELearningPlatform.Models;

namespace ELearningPlatform.Services.Interfaces;

public interface ICourseService
{
    Task<IReadOnlyList<Course>> GetAllAsync();
    Task<Course> GetByIdAsync(int id);
    Task<Course> GetDetailsAsync(int id);
    Task<Course> CreateAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(int id);
}
