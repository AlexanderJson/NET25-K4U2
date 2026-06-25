using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ContentApi.Infrastructure.Middleware;


    public sealed class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomMiddleware> _logger;

    public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
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
            // Logging minimal non-sensitive information to avoid leaking any secrets or response bodies
            _logger.LogError("Unhandled exception caught by CustomMiddleware: {ExceptionType} {Message}", ex.GetType().FullName, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
            var (statusCode, title, detail) = exception switch
            {
                ContentApi.Common.ExternalApiException extEx => TranslateExternalError(extEx),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", "Access is denied."),
                TaskCanceledException => (HttpStatusCode.RequestTimeout, "Request timeout", "The request timed out."),
                HttpRequestException => (HttpStatusCode.BadGateway, "External service error", "Failed to reach the external AI provider."),
                _ => (HttpStatusCode.InternalServerError, "Internal server error", "An unexpected error occurred.")
            };

        var problemDetails = new ProblemDetails
        {
            Type = $"https://example.com/probs/{statusCode.ToString().ToLower()}",
            Title = title,
            Status = (int)statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };

        context.Response.Clear();
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        if (exception is ContentApi.Common.ExternalApiException ext && ext.Headers != null && ext.Headers.TryGetValue("Retry-After", out var ra) && !string.IsNullOrEmpty(ra))
        {
            context.Response.Headers["Retry-After"] = ra;
        }

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(json);
    }

    private static (HttpStatusCode, string, string) TranslateExternalError(ContentApi.Common.ExternalApiException ex)
    {
        var code = ex.StatusCode;
        if (code == 401)
        {
            return (HttpStatusCode.BadGateway, "External provider unauthorized", "The configured AI provider rejected the request (401). Check API key and credentials.");
        }

        if (code == 429)
        {
            return (HttpStatusCode.TooManyRequests, "Rate limited by external provider", "The AI provider is rate limiting requests. Retry later.");
        }

        if (code >= 500 && code <= 599)
        {
            return (HttpStatusCode.BadGateway, "External provider error", "The AI provider returned a server error. Try again later.");
        }

        // Fallback: surface as Bad Gateway
        return (HttpStatusCode.BadGateway, "External service error", "An error occurred calling the external AI provider.");
    }
}