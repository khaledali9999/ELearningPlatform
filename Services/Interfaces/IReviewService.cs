using ELearningPlatform.Models;

namespace ELearningPlatform.Services.Interfaces;

public interface IReviewService
{
    /// <exception cref="Exceptions.NotFoundException">Student or course does not exist.</exception>
    /// <exception cref="Exceptions.InvalidRatingException">Rating is outside the allowed range.</exception>
    /// <exception cref="Exceptions.NotEnrolledException">Student is not enrolled in the course.</exception>
    /// <exception cref="Exceptions.DuplicateReviewException">Student already reviewed the course.</exception>
    Task<Review> AddReviewAsync(int studentId, int courseId, int rating, string? comment);

    Task<IReadOnlyList<Review>> GetCourseReviewsAsync(int courseId);
}
