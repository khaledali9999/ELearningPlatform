using ELearningPlatform.Exceptions;
using ELearningPlatform.Models;
using ELearningPlatform.Repositories.Interfaces;
using ELearningPlatform.Services.Interfaces;

namespace ELearningPlatform.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly ILogger<CourseService> _logger;

    public CourseService(
        ICourseRepository courseRepository,
        IInstructorRepository instructorRepository,
        ILogger<CourseService> logger)
    {
        _courseRepository = courseRepository;
        _instructorRepository = instructorRepository;
        _logger = logger;
    }

    public Task<IReadOnlyList<Course>> GetAllAsync() => _courseRepository.GetAllWithInstructorAsync();

    public async Task<Course> GetByIdAsync(int id) =>
        await _courseRepository.GetByIdAsync(id)
        ?? throw new NotFoundException(nameof(Course), id);

    public async Task<Course> GetDetailsAsync(int id) =>
        await _courseRepository.GetByIdWithDetailsAsync(id)
        ?? throw new NotFoundException(nameof(Course), id);

    public async Task<Course> CreateAsync(Course course)
    {
        await EnsureInstructorExistsAsync(course.InstructorId);

        await _courseRepository.AddAsync(course);
        _logger.LogInformation("Course {CourseId} '{CourseName}' created", course.Id, course.Name);
        return course;
    }

    public async Task UpdateAsync(Course course)
    {
        var existing = await GetByIdAsync(course.Id);
        await EnsureInstructorExistsAsync(course.InstructorId);

        existing.Name = course.Name;
        existing.Description = course.Description;
        existing.Price = course.Price;
        existing.MaxStudents = course.MaxStudents;
        existing.InstructorId = course.InstructorId;

        await _courseRepository.UpdateAsync(existing);
        _logger.LogInformation("Course {CourseId} updated", existing.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await GetByIdAsync(id);
        await _courseRepository.DeleteAsync(existing);
        _logger.LogInformation("Course {CourseId} deleted", id);
    }

    private async Task EnsureInstructorExistsAsync(int instructorId)
    {
        if (await _instructorRepository.GetByIdAsync(instructorId) is null)
            throw new NotFoundException(nameof(Instructor), instructorId);
    }
}
