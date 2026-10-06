using ELearningPlatform.Models;

namespace ELearningPlatform.Repositories.Interfaces;

public interface IReviewRepository : IRepository<Review>
{
    Task<bool> ExistsAsync(int studentId, int courseId);
    Task<IReadOnlyList<Review>> GetByCourseAsync(int courseId);
}
