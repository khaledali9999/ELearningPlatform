using ELearningPlatform.Models;

namespace ELearningPlatform.Repositories.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    Task<IReadOnlyList<Course>> GetAllWithInstructorAsync();
    Task<Course?> GetByIdWithDetailsAsync(int id);
}
