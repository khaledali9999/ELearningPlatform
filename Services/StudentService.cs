using ELearningPlatform.Exceptions;
using ELearningPlatform.Models;
using ELearningPlatform.Repositories.Interfaces;
using ELearningPlatform.Services.Interfaces;

namespace ELearningPlatform.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public Task<IReadOnlyList<Student>> GetAllAsync() => _studentRepository.GetAllAsync();

    public async Task<Student> GetByIdAsync(int id) =>
        await _studentRepository.GetByIdAsync(id)
        ?? throw new NotFoundException(nameof(Student), id);

    public async Task<Student> GetDetailsAsync(int id) =>
        await _studentRepository.GetByIdWithEnrollmentsAsync(id)
        ?? throw new NotFoundException(nameof(Student), id);

    public async Task<Student> CreateAsync(Student student)
    {
        await EnsureEmailIsUniqueAsync(student.Email, excludeStudentId: null);

        student.CreatedAt = DateTime.UtcNow;
        await _studentRepository.AddAsync(student);

        _logger.LogInformation("Student {StudentId} created ({Email})", student.Id, student.Email);
        return student;
    }

    public async Task UpdateAsync(Student student)
    {
        var existing = await GetByIdAsync(student.Id);
        await EnsureEmailIsUniqueAsync(student.Email, excludeStudentId: student.Id);

        existing.FullName = student.FullName;
        existing.Email = student.Email;
        existing.PhoneNumber = student.PhoneNumber;

        await _studentRepository.UpdateAsync(existing);
        _logger.LogInformation("Student {StudentId} updated", existing.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await GetByIdAsync(id);
        await _studentRepository.DeleteAsync(existing);
        _logger.LogInformation("Student {StudentId} deleted", id);
    }

    private async Task EnsureEmailIsUniqueAsync(string email, int? excludeStudentId)
    {
        if (await _studentRepository.EmailExistsAsync(email, excludeStudentId))
            throw new DuplicateEmailException(email);
    }
}
