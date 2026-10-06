using ELearningPlatform.Exceptions;
using ELearningPlatform.Services.Interfaces;
using ELearningPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearningPlatform.Controllers;

public class EnrollmentsController : Controller
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IStudentService _studentService;

    public EnrollmentsController(IEnrollmentService enrollmentService, IStudentService studentService)
    {
        _enrollmentService = enrollmentService;
        _studentService = studentService;
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int studentId, int courseId)
    {
        try
        {
            await _enrollmentService.EnrollAsync(studentId, courseId);
        }
        catch (BusinessRuleException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Details", "Courses", new { id = courseId });
        }

        TempData["Success"] = "Enrolled successfully.";
        return RedirectToAction(nameof(MyCourses), new { studentId });
    }

    public async Task<IActionResult> MyCourses(int? studentId)
    {
        var students = await _studentService.GetAllAsync();
        var enrollments = studentId is null
            ? Array.Empty<Models.Enrollment>()
            : await _enrollmentService.GetStudentEnrollmentsAsync(studentId.Value);

        return View(new MyCoursesViewModel
        {
            Students = students.Select(s => new SelectListItem(s.FullName, s.Id.ToString())),
            SelectedStudentId = studentId,
            Enrollments = enrollments
        });
    }
}
