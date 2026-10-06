namespace ELearningPlatform.Exceptions;

/// <summary>Base type for every exception we throw on purpose.</summary>
public abstract class AppException : Exception
{
    protected AppException(string message) : base(message)
    {
    }
}

/// <summary>A requested entity does not exist.</summary>
public class NotFoundException : AppException
{
    public NotFoundException(string entityName, int id)
        : base($"{entityName} with id {id} was not found.")
    {
    }
}

/// <summary>A business rule was violated (the request is valid but not allowed).</summary>
public abstract class BusinessRuleException : AppException
{
    protected BusinessRuleException(string message) : base(message)
    {
    }
}

public class AlreadyEnrolledException : BusinessRuleException
{
    public AlreadyEnrolledException(string studentName, string courseName)
        : base($"{studentName} is already enrolled in '{courseName}'.")
    {
    }
}

public class CourseFullException : BusinessRuleException
{
    public CourseFullException(string courseName, int maxStudents)
        : base($"'{courseName}' is full (maximum {maxStudents} students).")
    {
    }
}

public class NotEnrolledException : BusinessRuleException
{
    public NotEnrolledException(string studentName, string courseName)
        : base($"{studentName} is not enrolled in '{courseName}', so a review is not allowed.")
    {
    }
}

public class DuplicateReviewException : BusinessRuleException
{
    public DuplicateReviewException(string studentName, string courseName)
        : base($"{studentName} has already reviewed '{courseName}'.")
    {
    }
}

public class InvalidRatingException : BusinessRuleException
{
    public InvalidRatingException(int minRating, int maxRating)
        : base($"Rating must be between {minRating} and {maxRating}.")
    {
    }
}

public class DuplicateEmailException : BusinessRuleException
{
    public DuplicateEmailException(string email)
        : base($"A student with the email '{email}' already exists.")
    {
    }
}
