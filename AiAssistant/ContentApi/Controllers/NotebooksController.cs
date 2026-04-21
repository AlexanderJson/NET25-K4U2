
using ContentApi.Models;
using Microsoft.AspNetCore.Mvc;
using ContentApi.Services;
using ContentApi.DTO;
namespace ContentApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class NotebooksController
(
    ILogger<NotebooksController> logger,
    INotebookQueries query,
    INotebookService service
) : ControllerBase
{
    private readonly INotebookService _service = service;
    private readonly ILogger<NotebooksController> _logger = logger;
    private readonly INotebookQueries _query = query;
    private string? ApiVersion => HttpContext.GetRequestedApiVersion()?.ToString();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct">This is a struct that flags if a process should be stopped early. 
    /// F.example if the TCP connection is closed, a timeout or if user closes the browser.</param>
    /// <returns></returns>
    //[NotForProduction]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FullNotebook), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullNotebook>>GetById(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Fetching project {id}", id);
        var result = await _query.GetFullNotebookById(id, ct);
        if (result is null) return NotFound();
        return Ok(result);    
    }

    [HttpPost]
    [ProducesResponseType(typeof(FullNotebook), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FullNotebook>>Create([FromBody] CreateNotebookRequest request, CancellationToken ct)
    {
        var id = await _service.Create(request, ct);
        var result = await _query.GetFullNotebookById(id, ct);
        if (result is null) return StatusCode(500);
        return CreatedAtAction(
            nameof(GetById), // action needed to generate the location header url ("api/v1.0/Notebooks/{id})
            new { id, version = ApiVersion }, // id of created object + api version used with action to generate location url
            result); // body returned
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<NotebookResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotebookResponse>>> SearchByTitle(
        [FromQuery] string title,
        CancellationToken ct)
    {
        var notebooks = await _query.GetNotebookByTitle(title, ct);
        return Ok(notebooks);
    }

    [HttpGet("user/{userId:guid}/overview")]
    [ProducesResponseType(typeof(IReadOnlyList<NotebookOverview>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotebookOverview>>> GetOverview(
        Guid userId,
        CancellationToken ct)
    {
        var notebooks = await _query.GetNotebookOverview(userId, ct);
        return Ok(notebooks);
    }

    [HttpPatch("{id:guid}")]    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateNotebookRequest request,
        CancellationToken ct)
    {
        await _service.Update(id, request, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        await _service.Delete(id, ct);
        return NoContent();
    }

}
