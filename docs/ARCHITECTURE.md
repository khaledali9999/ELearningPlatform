# Architecture Diagram

![Architecture](architecture.svg)

```
        MVC (Views + ViewModels)
                 │
                 ▼
            Controllers          thin: bind input, call a service, return a result
                 │
                 ▼
             Services            ALL business rules + custom exceptions + logging
                 │
                 ▼
           Repositories          only EF Core queries (no business rules)
                 │
                 ▼
            AppDbContext
                 │
                 ▼
            SQL Server
```

## Enroll flow

```mermaid
sequenceDiagram
    participant C as EnrollmentsController
    participant S as EnrollmentService
    participant R as Repositories
    participant DB as SQL Server

    C->>S: EnrollAsync(studentId, courseId)
    S->>R: student exists?
    S->>R: course exists?
    S->>R: already enrolled?
    S->>R: course full?
    S->>R: AddAsync(enrollment)
    R->>DB: INSERT
    S-->>C: Enrollment (or custom exception)
```

## Review flow

```mermaid
sequenceDiagram
    participant C as ReviewsController
    participant S as ReviewService
    participant R as Repositories

    C->>S: AddReviewAsync(studentId, courseId, rating, comment)
    S->>R: student & course exist?
    S->>S: rating between 1 and 5?
    S->>R: student enrolled in course?
    S->>R: already reviewed?
    S->>R: AddAsync(review)
    S-->>C: Review (or custom exception)
```
