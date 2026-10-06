using ELearningPlatform.Exceptions;
using ELearningPlatform.Services.Interfaces;
using ELearningPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearningPlatform.Controllers;

public class ReviewsController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly ICourseService _courseService;
    private readonly IStudentService _studentService;

    public ReviewsController(
        IReviewService reviewService,
        ICourseService courseService,
        IStudentService studentService)
    {
        _reviewService = reviewService;
        _courseService = courseService;
        _studentService = studentService;
    }

    public async Task<IActionResult> Create(int courseId)
    {
        var course = await _courseService.GetByIdAsync(courseId);
        return View(await BuildFormAsync(new ReviewFormViewModel { CourseId = courseId }, course.Name));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReviewFormViewModel model)
    {
        var course = await _courseService.GetByIdAsync(model.CourseId);

        if (ModelState.IsValid)
        {
            try
            {
                await _reviewService.AddReviewAsync(model.StudentId, model.CourseId, model.Rating, model.Comment);
                TempData["Success"] = "Thank you! Your review was added.";
                return RedirectToAction("Details", "Courses", new { id = model.CourseId });
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        return View(await BuildFormAsync(model, course.Name));
    }

    private async Task<ReviewFormViewModel> BuildFormAsync(ReviewFormViewModel model, string courseName)
    {
        var students = await _studentService.GetAllAsync();
        model.CourseName = courseName;
        model.Students = students.Select(s => new SelectListItem(s.FullName, s.Id.ToString()));
        return model;
    }
}
