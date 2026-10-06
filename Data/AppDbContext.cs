using ELearningPlatform.Common;
using ELearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(e =>
        {
            e.Property(s => s.FullName).IsRequired().HasMaxLength(BusinessRules.Person.NameMaxLength);
            e.Property(s => s.Email).IsRequired().HasMaxLength(BusinessRules.Person.EmailMaxLength);
            e.Property(s => s.PhoneNumber).HasMaxLength(BusinessRules.Person.PhoneMaxLength);
            e.HasIndex(s => s.Email).IsUnique();
        });

        modelBuilder.Entity<Instructor>(e =>
        {
            e.Property(i => i.FullName).IsRequired().HasMaxLength(BusinessRules.Person.NameMaxLength);
            e.Property(i => i.Email).IsRequired().HasMaxLength(BusinessRules.Person.EmailMaxLength);
        });

        modelBuilder.Entity<Course>(e =>
        {
            e.Property(c => c.Name).IsRequired().HasMaxLength(BusinessRules.Course.NameMaxLength);
            e.Property(c => c.Description).HasMaxLength(BusinessRules.Course.DescriptionMaxLength);
            e.Property(c => c.Price).HasColumnType("decimal(18,2)");
            e.HasOne(c => c.Instructor)
             .WithMany(i => i.Courses)
             .HasForeignKey(c => c.InstructorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Lesson>(e =>
        {
            e.Property(l => l.Title).IsRequired().HasMaxLength(BusinessRules.Course.NameMaxLength);
        });

        // A student can be enrolled in a course only once (also enforced in the service).
        modelBuilder.Entity<Enrollment>(e =>
        {
            e.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();
        });

        // A student can review a course only once (also enforced in the service).
        modelBuilder.Entity<Review>(e =>
        {
            e.Property(r => r.Comment).HasMaxLength(BusinessRules.Review.MaxCommentLength);
            e.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();
            e.HasOne(r => r.Student)
             .WithMany(s => s.Reviews)
             .HasForeignKey(r => r.StudentId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.Course)
             .WithMany(c => c.Reviews)
             .HasForeignKey(r => r.CourseId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Instructor>().HasData(
            new Instructor { Id = 1, FullName = "Ahmed Hassan", Email = "ahmed.hassan@example.com", Bio = "Senior .NET trainer" },
            new Instructor { Id = 2, FullName = "Sara Ali", Email = "sara.ali@example.com", Bio = "Software architecture mentor" });
    }
}
