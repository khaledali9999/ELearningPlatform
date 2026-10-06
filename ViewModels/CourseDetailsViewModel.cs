using ELearningPlatform.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearningPlatform.ViewModels;

public class CourseDetailsViewModel
{
    public required Course Course { get; init; }
    public IEnumerable<SelectListItem> Students { get; init; } = Enumerable.Empty<SelectListItem>();

    public int EnrolledCount => Course.Enrollments.Count;
    public bool IsFull => EnrolledCount >= Course.MaxStudents;
    public double? AverageRating => Course.Reviews.Count == 0 ? null : Course.Reviews.Average(r => r.Rating);
}
