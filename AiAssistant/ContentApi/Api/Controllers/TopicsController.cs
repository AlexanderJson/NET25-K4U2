using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TopicsController(
    ISender sender,
    ILogger<TopicsController> logger)
    : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly ILogger<TopicsController> _logger = logger;

    [HttpPost("generate")]
    [ProducesResponseType(typeof(IReadOnlyList<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenerateTopicsResponse>> Generate(
        [FromBody] GenerateTopicsRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Generating topics for notebook!");

        var command = new GenerateTopicsCommand(request.NotebookId);

        try
        {
            var result = await _sender.Send(command, ct);
            return Ok(result);
        }
        catch (NotebookNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpPost]
    public async Task<IActionResult> TestProxy(CancellationToken ct)
    {
        var result = await _sender.Send(new DebugProxyCommand(), ct);
        return Content(result, "application/json");
    }


}


public sealed record DebugProxyCommand() : IRequest<string>;



public sealed class DebugProxyHandler : IRequestHandler<DebugProxyCommand, string>
{
    public async Task<string> Handle(DebugProxyCommand request, CancellationToken ct)
    {
        var prompt = "Say hello from Gemini in one short sentence.";

        Console.WriteLine("=== MEDIATR DEBUG START ===");
        Console.WriteLine("PROMPT:");
        Console.WriteLine(prompt);

        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5002")
        };

        var payload = new
        {
            prompt = prompt
        };

        var json = JsonSerializer.Serialize(payload);

        Console.WriteLine("JSON SENT TO PROXY:");
        Console.WriteLine(json);

        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("api/proxy", content, ct);

        Console.WriteLine($"STATUS CODE: {(int)response.StatusCode} {response.StatusCode}");

        var rawResponse = await response.Content.ReadAsStringAsync(ct);

        Console.WriteLine("RAW RESPONSE FROM PROXY:");
        Console.WriteLine(rawResponse);
        Console.WriteLine("=== MEDIATR DEBUG END ===");

        return rawResponse;
    }
}