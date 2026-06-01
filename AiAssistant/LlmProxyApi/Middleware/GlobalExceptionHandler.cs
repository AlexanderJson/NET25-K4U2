using System.Diagnostics;
using LlmProxyApi.Service;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LlmProxyApi.Middleware;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext ctx,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var error = StatusCodeMappings.Map(exception);

        if (exception is GeminiApiException geminiException)
        {
            logger.LogError(
                exception,
                "Gemini API error. GeminiStatus: {GeminiStatus}. GeminiCode: {GeminiCode}",
                geminiException.GeminiStatus,
                geminiException.GeminiCode);
        }
        else
        {
            logger.LogError(
                exception,
                "Unhandled exception! ExceptionType: {ExceptionType}",
                exception.GetType().Name);
        }

        var problem = new ProblemDetails
        {
            Status = error.StatusCode,
            Title = error.Title,
            Detail = environment.IsDevelopment()
                ? exception.Message
                : error.Detail,
            Instance = ctx.Request.Path
        };

        problem.Extensions["traceId"] =
            Activity.Current?.Id ?? ctx.TraceIdentifier;

        if (environment.IsDevelopment())
        {
            problem.Extensions["exceptionType"] = exception.GetType().FullName;
        }

        ctx.Response.StatusCode = error.StatusCode;

        var written = await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = ctx,
                Exception = exception,
                ProblemDetails = problem
            });

        if (!written && !ctx.Response.HasStarted)
        {
            ctx.Response.ContentType = "text/plain";

            await ctx.Response.WriteAsync(
                environment.IsDevelopment()
                    ? exception.Message
                    : error.Detail,
                cancellationToken);
        }

        return true;
    }
}

public static class StatusCodeMappings
{
    public static ApiError Map(Exception exception)
    {
        return exception switch
        {
            GeminiApiException geminiException => MapGeminiException(geminiException),

            ArgumentException => ApiErrors.InvalidRequest,

            _ => ApiErrors.Unexpected
        };
    }

    private static ApiError MapGeminiException(GeminiApiException exception)
    {
        return exception.GeminiStatus switch
        {
            GeminiStatus.InvalidArgument => ApiErrors.AiRequestRejected,

            GeminiStatus.ResourceExhausted => ApiErrors.AiServiceUnavailable,

            GeminiStatus.Unavailable => ApiErrors.AiServiceUnavailable,

            GeminiStatus.Internal => ApiErrors.AiServiceUnavailable,

            GeminiStatus.FailedPrecondition => ApiErrors.AiServiceUnavailable,

            GeminiStatus.DeadlineExceeded => ApiErrors.AiGatewayTimeout,

            GeminiStatus.PermissionDenied => ApiErrors.AiBadGateway,

            GeminiStatus.NotFound => ApiErrors.AiBadGateway,

            _ => ApiErrors.AiBadGateway
        };
    }
}

public sealed record ApiError(
    int StatusCode,
    string Title,
    string Detail);

public static class ApiErrors
{
    public static readonly ApiError InvalidRequest = new(
        StatusCodes.Status400BadRequest,
        "Invalid request.",
        "The request could not be processed because it contains invalid data.");

    public static readonly ApiError AiRequestRejected = new(
        StatusCodes.Status400BadRequest,
        "AI request rejected.",
        "The AI service could not process the request.");

    public static readonly ApiError AiBadGateway = new(
        StatusCodes.Status502BadGateway,
        "AI service error.",
        "The AI service returned an invalid or unexpected response.");

    public static readonly ApiError AiServiceUnavailable = new(
        StatusCodes.Status503ServiceUnavailable,
        "AI service unavailable.",
        "The AI service is temporarily unavailable. Please try again later.");

    public static readonly ApiError AiGatewayTimeout = new(
        StatusCodes.Status504GatewayTimeout,
        "AI service timeout.",
        "The AI service did not respond in time. Please try again later.");

    public static readonly ApiError Unexpected = new(
        StatusCodes.Status500InternalServerError,
        "Unexpected error.",
        "An unexpected server error occurred.");
}