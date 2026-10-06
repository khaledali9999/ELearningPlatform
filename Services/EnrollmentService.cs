using ELearningPlatform.Exceptions;
using ELearningPlatform.Models;
using ELearningPlatform.Repositories.Interfaces;
using ELearningPlatform.Services.Interfaces;

namespace ELearningPlatform.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<EnrollmentService> _logger;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        ILogger<EnrollmentService> logger)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _logger = logger;
    }

    public async Task<Enrollment> EnrollAsync(int studentId, int courseId)
    {
        // 1. Does the student exist?
        var student = await _studentRepository.GetByIdAsync(studentId)
            ?? throw new NotFoundException(nameof(Student), studentId);

        // 2. Does the course exist?
        var course = await _courseRepository.GetByIdAsync(courseId)
            ?? throw new NotFoundException(nameof(Course), courseId);

        // 3. Is the student already enrolled?
        if (await _enrollmentRepository.ExistsAsync(studentId, courseId))
        {
            _logger.LogWarning("Student {StudentId} tried to enroll twice in course {CourseId}", studentId, courseId);
            throw new AlreadyEnrolledException(student.FullName, course.Name);
        }

        // 4. Is the course full?
        var enrolledCount = await _enrollmentRepository.CountByCourseAsync(courseId);
        if (enrolledCount >= course.MaxStudents)
        {
            _logger.LogWarning("Course {CourseId} is full ({Count}/{Max})", courseId, enrolledCount, course.MaxStudents);
            throw new CourseFullException(course.Name, course.MaxStudents);
        }

        // 5. Enroll the student.
        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledAt = DateTime.UtcNow
        };
        await _enrollmentRepository.AddAsync(enrollment);

        _logger.LogInformation("Student {StudentId} enrolled in course {CourseId}", studentId, courseId);
        return enrollment;
    }

    public async Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsAsync(int studentId)
    {
        if (await _studentRepository.GetByIdAsync(studentId) is null)
            throw new NotFoundException(nameof(Student), studentId);

        return await _enrollmentRepository.GetByStudentAsync(studentId);
    }
}
