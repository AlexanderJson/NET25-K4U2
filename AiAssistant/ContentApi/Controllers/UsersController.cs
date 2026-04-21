
using ContentApi.Models;
using Microsoft.AspNetCore.Mvc;
using ContentApi.Services;
using ContentApi.DTO;
namespace ContentApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersControllers
(
    ILogger<UsersControllers> logger,
    IUserQueries query,
    IUserService service
) : ControllerBase
{
    private readonly IUserService _service = service;
    private readonly ILogger<UsersControllers> _logger = logger;
    private readonly IUserQueries _query = query;
    private string? ApiVersion => HttpContext.GetRequestedApiVersion()?.ToString();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct">This is a struct that flags if a process should be stopped early. 
    /// F.example if the TCP connection is closed, a timeout or if user closes the browser.</param>
    /// <returns></returns>
    //[NotForProduction]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserSummary>>GetById(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Fetching project {id}", id);
        var result = await _query.GetUserSummaryById(id, ct);
        if (result is null) return NotFound();
        return Ok(result);    
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserSummary), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FullNotebook>>Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var id = await _service.Create(request, ct);
        var result = await _query.GetUserSummaryById(id, ct);
        if (result is null) return StatusCode(500);
        return CreatedAtAction(
            nameof(GetById), 
            new { id, version = ApiVersion }, 
            result); 
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<UserSummary>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserSummary>>> SearchByTitle(
        [FromQuery] string searchTerm,
        CancellationToken ct)
    {
        var notebooks = await _query.SearchUsers(searchTerm, ct);
        return Ok(notebooks);
    }

    [HttpPatch("{id:guid}")]    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserRequest request,
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
