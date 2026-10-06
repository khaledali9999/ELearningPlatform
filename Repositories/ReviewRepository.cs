using ELearningPlatform.Data;
using ELearningPlatform.Models;
using ELearningPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsAsync(int studentId, int courseId) =>
        await Set.AnyAsync(r => r.StudentId == studentId && r.CourseId == courseId);

    public async Task<IReadOnlyList<Review>> GetByCourseAsync(int courseId) =>
        await Set
            .Include(r => r.Student)
            .AsNoTracking()
            .Where(r => r.CourseId == courseId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
}
