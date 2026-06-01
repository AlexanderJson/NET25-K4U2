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

        return await problemDetailsService.TryWriteAsync
        (
            new ProblemDetailsContext
            {
                HttpContext = ctx,
                Exception = exception,
                ProblemDetails = problem
            }
        );
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
 
}


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
}