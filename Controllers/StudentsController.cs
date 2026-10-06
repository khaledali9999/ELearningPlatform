using ELearningPlatform.Exceptions;
using ELearningPlatform.Services.Interfaces;
using ELearningPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ELearningPlatform.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task<IActionResult> Index() => View(await _studentService.GetAllAsync());

    public async Task<IActionResult> Details(int id) => View(await _studentService.GetDetailsAsync(id));

    public IActionResult Create() => View(new StudentFormViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _studentService.CreateAsync(model.ToEntity());
        }
        catch (DuplicateEmailException ex)
        {
            ModelState.AddModelError(nameof(model.Email), ex.Message);
            return View(model);
        }

        TempData["Success"] = "Student created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id) =>
        View(StudentFormViewModel.FromEntity(await _studentService.GetByIdAsync(id)));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentFormViewModel model)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _studentService.UpdateAsync(model.ToEntity());
        }
        catch (DuplicateEmailException ex)
        {
            ModelState.AddModelError(nameof(model.Email), ex.Message);
            return View(model);
        }

        TempData["Success"] = "Student updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id) => View(await _studentService.GetByIdAsync(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _studentService.DeleteAsync(id);
        TempData["Success"] = "Student deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
