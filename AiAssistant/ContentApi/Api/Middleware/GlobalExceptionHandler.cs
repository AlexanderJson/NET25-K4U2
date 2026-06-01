using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContentApi.Api.Middleware;

public sealed class GlobalExceptionHandler
(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync
    (
        HttpContext ctx, 
        Exception exception, 
        CancellationToken cancellationToken
    )
    {
        // First we map the exception to our ApiError record
        var error = StatusCodeMappings.Map(exception);

        logger.LogError(exception,"Unhandled Exception! {ExceptionType}",exception.GetType().Name);
        var problem = new ProblemDetails
        {
            Status = error.StatusCode,
            Title = error.Title,
            Detail = environment.IsDevelopment()
                ? exception.Message
                : error.Detail,
            Instance = ctx.Request.Path
        };
        problem.Extensions["traceId"] = Activity.Current?.Id ??  ctx.TraceIdentifier;
        if(environment.IsDevelopment()) problem.Extensions["exceptionType"] = exception.GetType().FullName;
        
        ctx.Response.StatusCode = error.StatusCode;

        
        await problemDetailsService.TryWriteAsync
        (
            new ProblemDetailsContext
            {
                HttpContext = ctx,
                Exception = exception,
                ProblemDetails = problem
            }
        );

        return true;
    }
}


public static class StatusCodeMappings
{
    public static ApiError Map(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => ApiErrors.InvalidRequest,

            ArgumentException => ApiErrors.InvalidRequest,

            KeyNotFoundException => ApiErrors.ResourceNotFound,

            DbUpdateConcurrencyException => ApiErrors.ConcurrencyConflict,

            _ => ApiErrors.Unexpected
        };
    }
    private static ApiError MapGeminiException(GeminiApiException exception)
    {
        return exception.GeminiStatus switch
        {
            "RESOURCE_EXHAUSTED" => ApiErrors.AiServiceUnavailable,
            "UNAVAILABLE" => ApiErrors.AiServiceUnavailable,
            "INTERNAL" => ApiErrors.AiServiceUnavailable,

            "DEADLINE_EXCEEDED" => ApiErrors.AiGatewayTimeout,

            "INVALID_ARGUMENT" => ApiErrors.AiBadGateway,
            "FAILED_PRECONDITION" => ApiErrors.AiServiceUnavailable,
            "PERMISSION_DENIED" => ApiErrors.AiBadGateway,
            "NOT_FOUND" => ApiErrors.AiBadGateway,

            _ => ApiErrors.AiBadGateway
        };
    }
 
}

// I just  mapped this from gemini api docs: https://ai.google.dev/gemini-api/docs/troubleshooting
public sealed record GeminiError(int Code, string Message, string Status);
 

public sealed record ApiError(int StatusCode, string Title, string Detail);
public static class ApiErrors
{
    public static readonly ApiError InvalidRequest = new(
        StatusCodes.Status400BadRequest,
        "Invalid request.",
        "The request could not be processed because it contains invalid data.");

    //todo senare
    public static readonly ApiError ValidationFailed = new(
        StatusCodes.Status422UnprocessableEntity,
        "Validation failed.",
        "One or more validation errors occurred.");

    public static readonly ApiError ResourceNotFound = new(
        StatusCodes.Status404NotFound,
        "Resource not found.",
        "The requested resource was not found.");

    public static readonly ApiError ConcurrencyConflict = new(
        StatusCodes.Status409Conflict,
        "Concurrency conflict.",
        "The resource was modified by another request.");

    public static readonly ApiError Unexpected = new(
        StatusCodes.Status500InternalServerError,
        "Unexpected error.",
        "An unexpected server error occurred.");

    // Todo: bör skiljas sen
    public static readonly ApiError AiServiceUnavailable = new(
    StatusCodes.Status503ServiceUnavailable,
    "AI service unavailable.",
    "The AI service is temporarily unavailable. Please try again later.");

    public static readonly ApiError AiGatewayTimeout = new(
        StatusCodes.Status504GatewayTimeout,
        "AI service timeout.",
        "The AI service did not respond in time. Please try again later.");

    public static readonly ApiError AiBadGateway = new(
        StatusCodes.Status502BadGateway,
        "AI service error.",
        "The AI service returned an invalid or unexpected response.");
}

public sealed class GeminiApiException : Exception
{
    public int? GeminiStatusCode { get; }
    public string? GeminiStatus { get; }

    public GeminiApiException(
        string message,
        int? geminiStatusCode = null,
        string? geminiStatus = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        GeminiStatusCode = geminiStatusCode;
        GeminiStatus = geminiStatus;
    } 
}