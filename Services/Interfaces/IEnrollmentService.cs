using ELearningPlatform.Models;

namespace ELearningPlatform.Services.Interfaces;

public interface IEnrollmentService
{
    /// <exception cref="Exceptions.NotFoundException">Student or course does not exist.</exception>
    /// <exception cref="Exceptions.AlreadyEnrolledException">Student is already enrolled.</exception>
    /// <exception cref="Exceptions.CourseFullException">Course reached its maximum capacity.</exception>
    Task<Enrollment> EnrollAsync(int studentId, int courseId);

    Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsAsync(int studentId);
}
