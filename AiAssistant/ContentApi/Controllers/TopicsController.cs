using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

public sealed record GenerateTopicsCommand(Guid notebookId);
public sealed record GenerateTopicsResponse(string Result);
public sealed record GenerateTopicsRequest(Guid NotebookId);
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TopicsController
(
    NotebookWorkflowService workflow,
    ILogger<TopicsController> logger
) : ControllerBase
{
    private readonly NotebookWorkflowService _workflow = workflow;
    private readonly ILogger<TopicsController> _logger = logger;

    [HttpPost("generate")]
    [ProducesResponseType(typeof(IReadOnlyList<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<Guid>>> Generate(
        [FromBody] GenerateTopicsRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Generating AI topics for notebook {NotebookId}",
            request.NotebookId);

        try
        {
            var result = await _workflow.GenerateTopicsFromGemini(
                request.NotebookId,
                ct);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) when (ex.Message.Contains("null"))
        {
            return NotFound();
        }
    }
}