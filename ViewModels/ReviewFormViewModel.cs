using System.ComponentModel.DataAnnotations;
using ELearningPlatform.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELearningPlatform.ViewModels;

public class ReviewFormViewModel
{
    public int CourseId { get; set; }
    [ValidateNever]
    public string CourseName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Please choose a student.")]
    [Display(Name = "Student")]
    public int StudentId { get; set; }

    [Range(BusinessRules.Review.MinRating, BusinessRules.Review.MaxRating)]
    public int Rating { get; set; } = BusinessRules.Review.MaxRating;

    [StringLength(BusinessRules.Review.MaxCommentLength)]
    public string? Comment { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> Students { get; set; } = Enumerable.Empty<SelectListItem>();
}
