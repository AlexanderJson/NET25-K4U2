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
    public async Task<ActionResult<GeminiResponse>> Generate([FromBody] GeminiRequest request)
    {
        Console.WriteLine($"REQUEST: {request.Prompt}");
        var response = await _service.GetAiContent(request);
        Console.WriteLine($"RESPONSE: {response.Result}");

        return Ok(response);
    }
}