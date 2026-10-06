using ELearningPlatform.Data;
using ELearningPlatform.Models;
using ELearningPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Course>> GetAllWithInstructorAsync() =>
        await Set
            .Include(c => c.Instructor)
            .Include(c => c.Enrollments)
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

    public async Task<Course?> GetByIdWithDetailsAsync(int id) =>
        await Set
            .Include(c => c.Instructor)
            .Include(c => c.Lessons)
            .Include(c => c.Enrollments)
            .Include(c => c.Reviews)
                .ThenInclude(r => r.Student)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == id);
}
