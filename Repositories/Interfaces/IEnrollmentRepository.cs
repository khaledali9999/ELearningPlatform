using ELearningPlatform.Models;

namespace ELearningPlatform.Repositories.Interfaces;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<bool> ExistsAsync(int studentId, int courseId);
    Task<int> CountByCourseAsync(int courseId);
    Task<IReadOnlyList<Enrollment>> GetByStudentAsync(int studentId);
}
