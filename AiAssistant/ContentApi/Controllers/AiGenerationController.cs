
using AiAssistant.ContentApi.DTO;
using ContentApi.Models;
using Microsoft.AspNetCore.Mvc;
using ContentApi.Services;
namespace ContentApi.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AiGenerationController
(
    IAiGenerationService<AiGenerationRequest, 
    AiGenerationResponse, AiGeneration> service,
    ILogger<AiGenerationController> logger
) : ControllerBase
{
    private readonly IAiGenerationService<AiGenerationRequest, AiGenerationResponse, AiGeneration> _service = service;
    private readonly ILogger<AiGenerationController> _logger = logger;

    [HttpPost]
    [ProducesResponseType(typeof(AiGenerationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AiGenerationResponse>>Create([FromBody] AiGenerationRequest request, CancellationToken ct)
    {
        var result = await _service.Create(request,ct);
        return CreatedAtAction(
            nameof(_service.GetById),
            new { id = result.Id },
            result
        );
    }


}

