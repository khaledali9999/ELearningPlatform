using ELearningPlatform.Services.Interfaces;
using ELearningPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearningPlatform.Controllers;

public class CoursesController : Controller
{
    private readonly ICourseService _courseService;
    private readonly IInstructorService _instructorService;
    private readonly IStudentService _studentService;

    public CoursesController(
        ICourseService courseService,
        IInstructorService instructorService,
        IStudentService studentService)
    {
        _courseService = courseService;
        _instructorService = instructorService;
        _studentService = studentService;
    }

    public async Task<IActionResult> Index() => View(await _courseService.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        var course = await _courseService.GetDetailsAsync(id);
        var students = await _studentService.GetAllAsync();

        return View(new CourseDetailsViewModel
        {
            Course = course,
            Students = students.Select(s => new SelectListItem(s.FullName, s.Id.ToString()))
        });
    }

    public async Task<IActionResult> Create() => View(await WithInstructorsAsync(new CourseFormViewModel()));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await WithInstructorsAsync(model));

        await _courseService.CreateAsync(model.ToEntity());
        TempData["Success"] = "Course created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        return View(await WithInstructorsAsync(CourseFormViewModel.FromEntity(course)));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CourseFormViewModel model)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(await WithInstructorsAsync(model));

        await _courseService.UpdateAsync(model.ToEntity());
        TempData["Success"] = "Course updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id) => View(await _courseService.GetByIdAsync(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _courseService.DeleteAsync(id);
        TempData["Success"] = "Course deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<CourseFormViewModel> WithInstructorsAsync(CourseFormViewModel model)
    {
        var instructors = await _instructorService.GetAllAsync();
        model.Instructors = instructors.Select(i => new SelectListItem(i.FullName, i.Id.ToString()));
        return model;
    }
}
