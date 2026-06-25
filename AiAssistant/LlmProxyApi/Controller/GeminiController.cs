using LlmProxyApi.Service;
using Microsoft.AspNetCore.Mvc;
namespace LlmProxyApi.Controller;

[ApiController]
[Route("api/proxy")]
public class GeminiController(GeminiService service) : ControllerBase
{
    private readonly GeminiService _service = service;

    [HttpPost]
    [ProducesResponseType(typeof(GeminiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GeminiResponse>> Generate([FromBody] GeminiRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _service.GetAiContent(request, ct);
            return Ok(response);
        }
        catch (ExternalApiException ex)
        {
            //TODO lägg på mer?
            var (statusCode, title, detail) = ex.StatusCode switch
            {
                401 => ((int)System.Net.HttpStatusCode.BadGateway, "External provider unauthorized", "The configured AI provider rejected the request (401). Check API key and credentials."),
                429 => ((int)System.Net.HttpStatusCode.TooManyRequests, "Rate limited by external provider", "The AI provider is rate limiting requests. Retry later."),
                var c when c >= 500 && c <= 599 => ((int)System.Net.HttpStatusCode.BadGateway, "External provider error", "The AI provider returned a server error. Try again later."),
                _ => ((int)System.Net.HttpStatusCode.BadGateway, "External service error", "An error occurred calling the external AI provider.")
            };

            var problemDetails = new ProblemDetails
            {
                Type = $"https://example.com/probs/{statusCode}",
                Title = title,
                Status = statusCode,
                Detail = detail,
                Instance = HttpContext.Request.Path
            };

            var result = new ObjectResult(problemDetails) { StatusCode = statusCode };

            if (ex.Headers != null && ex.Headers.TryGetValue("Retry-After", out var ra) && !string.IsNullOrEmpty(ra))
            {
                HttpContext.Response.Headers["Retry-After"] = ra;
            }

            return result;
        }
    }
}