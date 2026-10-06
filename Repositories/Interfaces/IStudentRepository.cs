using ELearningPlatform.Models;

namespace ELearningPlatform.Repositories.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByIdWithEnrollmentsAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeStudentId = null);
}
