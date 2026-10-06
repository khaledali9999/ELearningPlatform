using ELearningPlatform.Common;
using ELearningPlatform.Exceptions;
using ELearningPlatform.Models;
using ELearningPlatform.Repositories.Interfaces;
using ELearningPlatform.Services.Interfaces;

namespace ELearningPlatform.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(
        IReviewRepository reviewRepository,
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        ILogger<ReviewService> logger)
    {
        _reviewRepository = reviewRepository;
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _logger = logger;
    }

    public async Task<Review> AddReviewAsync(int studentId, int courseId, int rating, string? comment)
    {
        var student = await _studentRepository.GetByIdAsync(studentId)
            ?? throw new NotFoundException(nameof(Student), studentId);

        var course = await _courseRepository.GetByIdAsync(courseId)
            ?? throw new NotFoundException(nameof(Course), courseId);

        if (rating < BusinessRules.Review.MinRating || rating > BusinessRules.Review.MaxRating)
            throw new InvalidRatingException(BusinessRules.Review.MinRating, BusinessRules.Review.MaxRating);

        if (!await _enrollmentRepository.ExistsAsync(studentId, courseId))
        {
            _logger.LogWarning("Student {StudentId} tried to review course {CourseId} without enrollment", studentId, courseId);
            throw new NotEnrolledException(student.FullName, course.Name);
        }

        if (await _reviewRepository.ExistsAsync(studentId, courseId))
        {
            _logger.LogWarning("Student {StudentId} tried to review course {CourseId} twice", studentId, courseId);
            throw new DuplicateReviewException(student.FullName, course.Name);
        }

        var review = new Review
        {
            StudentId = studentId,
            CourseId = courseId,
            Rating = rating,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        await _reviewRepository.AddAsync(review);

        _logger.LogInformation("Student {StudentId} reviewed course {CourseId} with rating {Rating}", studentId, courseId, rating);
        return review;
    }

    public async Task<IReadOnlyList<Review>> GetCourseReviewsAsync(int courseId)
    {
        if (await _courseRepository.GetByIdAsync(courseId) is null)
            throw new NotFoundException(nameof(Course), courseId);

        return await _reviewRepository.GetByCourseAsync(courseId);
    }
}
