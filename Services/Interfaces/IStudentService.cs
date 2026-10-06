using ELearningPlatform.Models;

namespace ELearningPlatform.Services.Interfaces;

public interface IStudentService
{
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<Student> GetByIdAsync(int id);
    Task<Student> GetDetailsAsync(int id);
    Task<Student> CreateAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(int id);
}
