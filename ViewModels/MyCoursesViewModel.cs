using ELearningPlatform.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearningPlatform.ViewModels;

public class MyCoursesViewModel
{
    public IEnumerable<SelectListItem> Students { get; init; } = Enumerable.Empty<SelectListItem>();
    public int? SelectedStudentId { get; init; }
    public IReadOnlyList<Enrollment> Enrollments { get; init; } = Array.Empty<Enrollment>();
}
