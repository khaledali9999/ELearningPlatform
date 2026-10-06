using System.Net;
using System.Net.Mime;
using ELearningPlatform.Exceptions;

namespace ELearningPlatform.Middleware;

/// <summary>Global exception handler: logs every unhandled exception and returns a clean error page.</summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = MapException(exception);

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
        else
            _logger.LogWarning("Handled {ExceptionType}: {Message}", exception.GetType().Name, exception.Message);

        if (context.Response.HasStarted)
            return;

        context.Response.Clear();
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = MediaTypeNames.Text.Html;

        var safeMessage = System.Net.WebUtility.HtmlEncode(message);
        await context.Response.WriteAsync(
            $"<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>Error {(int)statusCode}</title>" +
            "<link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css\"></head>" +
            $"<body class=\"container py-5\"><div class=\"alert alert-danger\"><h4>Error {(int)statusCode}</h4><p>{safeMessage}</p></div>" +
            "<a class=\"btn btn-primary\" href=\"/\">Back to home</a></body></html>");
    }

    private static (HttpStatusCode StatusCode, string Message) MapException(Exception exception) => exception switch
    {
        NotFoundException => (HttpStatusCode.NotFound, exception.Message),
        BusinessRuleException => (HttpStatusCode.Conflict, exception.Message),
        _ => (HttpStatusCode.InternalServerError, "Something went wrong. Please try again later.")
    };
}
