using System.ComponentModel.DataAnnotations;
using ELearningPlatform.Common;
using ELearningPlatform.Models;

namespace ELearningPlatform.ViewModels;

public class StudentFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(BusinessRules.Person.NameMaxLength)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(BusinessRules.Person.EmailMaxLength)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(BusinessRules.Person.PhoneMaxLength)]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    public Student ToEntity() => new()
    {
        Id = Id,
        FullName = FullName.Trim(),
        Email = Email.Trim().ToLowerInvariant(),
        PhoneNumber = PhoneNumber?.Trim()
    };

    public static StudentFormViewModel FromEntity(Student student) => new()
    {
        Id = student.Id,
        FullName = student.FullName,
        Email = student.Email,
        PhoneNumber = student.PhoneNumber
    };
}
