using ELearningPlatform.Data;
using ELearningPlatform.Models;
using ELearningPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsAsync(int studentId, int courseId) =>
        await Set.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

    public async Task<int> CountByCourseAsync(int courseId) =>
        await Set.CountAsync(e => e.CourseId == courseId);

    public async Task<IReadOnlyList<Enrollment>> GetByStudentAsync(int studentId) =>
        await Set
            .Include(e => e.Course)
                .ThenInclude(c => c!.Instructor)
            .AsNoTracking()
            .Where(e => e.StudentId == studentId)
            .OrderByDescending(e => e.EnrolledAt)
            .ToListAsync();
}
