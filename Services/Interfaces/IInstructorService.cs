using ELearningPlatform.Models;

namespace ELearningPlatform.Services.Interfaces;

public interface IInstructorService
{
    Task<IReadOnlyList<Instructor>> GetAllAsync();
}
