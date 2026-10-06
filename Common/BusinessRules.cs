namespace ELearningPlatform.Common;

/// <summary>Single place for every business constant (no magic numbers in the code).</summary>
public static class BusinessRules
{
    public static class Review
    {
        public const int MinRating = 1;
        public const int MaxRating = 5;
        public const int MaxCommentLength = 1000;
    }

    public static class Course
    {
        public const int MinStudents = 1;
        public const int MaxStudentsLimit = 1000;
        public const int NameMaxLength = 150;
        public const int DescriptionMaxLength = 2000;
        public const double MaxPrice = 100000;
    }

    public static class Person
    {
        public const int NameMaxLength = 100;
        public const int EmailMaxLength = 150;
        public const int PhoneMaxLength = 20;
    }
}
