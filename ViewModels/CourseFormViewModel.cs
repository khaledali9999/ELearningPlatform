using System.ComponentModel.DataAnnotations;
using ELearningPlatform.Common;
using ELearningPlatform.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearningPlatform.ViewModels;

public class CourseFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(BusinessRules.Course.NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(BusinessRules.Course.DescriptionMaxLength)]
    public string Description { get; set; } = string.Empty;

    [Range(0, BusinessRules.Course.MaxPrice)]
    public decimal Price { get; set; }

    [Range(BusinessRules.Course.MinStudents, BusinessRules.Course.MaxStudentsLimit)]
    [Display(Name = "Maximum students")]
    public int MaxStudents { get; set; } = BusinessRules.Course.MinStudents;

    [Range(1, int.MaxValue, ErrorMessage = "Please choose an instructor.")]
    [Display(Name = "Instructor")]
    public int InstructorId { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> Instructors { get; set; } = Enumerable.Empty<SelectListItem>();

    public Course ToEntity() => new()
    {
        Id = Id,
        Name = Name.Trim(),
        Description = Description.Trim(),
        Price = Price,
        MaxStudents = MaxStudents,
        InstructorId = InstructorId
    };

    public static CourseFormViewModel FromEntity(Course course) => new()
    {
        Id = course.Id,
        Name = course.Name,
        Description = course.Description,
        Price = course.Price,
        MaxStudents = course.MaxStudents,
        InstructorId = course.InstructorId
    };
}
