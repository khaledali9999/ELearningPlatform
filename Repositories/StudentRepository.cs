using ELearningPlatform.Data;
using ELearningPlatform.Models;
using ELearningPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetByIdWithEnrollmentsAsync(int id) =>
        await Set
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<bool> EmailExistsAsync(string email, int? excludeStudentId = null) =>
        await Set.AnyAsync(s => s.Email == email && (excludeStudentId == null || s.Id != excludeStudentId));
}
