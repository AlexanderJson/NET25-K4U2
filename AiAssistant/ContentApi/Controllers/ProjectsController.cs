
using ContentApi.Models;
using Microsoft.AspNetCore.Mvc;
using ContentApi.Services;
namespace ContentApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProjectsController
(
    IProjectService<ProjectRequest, 
    ProjectResponse, Project> service,
    ILogger<ProjectsController> logger
) : ControllerBase
{
    private readonly IProjectService<ProjectRequest, ProjectResponse, Project> _service = service;
    private readonly ILogger<ProjectsController> _logger = logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct">This is a struct that flags if a process should be stopped early. 
    /// F.example if the TCP connection is closed, a timeout or if user closes the browser.</param>
    /// <returns></returns>
    //[NotForProduction]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>>GetAll(CancellationToken ct)
    {
        _logger.LogInformation("Fetching projects");
        var result = await _service.GetAll(ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>>GetById(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Fetching project {ProjectId}", id);
        var result = await _service.GetById(id,ct);
        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>>Create([FromBody] ProjectRequest request, CancellationToken ct)
    {
        var result = await _service.Create(request,ct);
        return CreatedAtAction(
            nameof(_service.GetById),
            new { id = result.Id },
            result
        );
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> Update(
        Guid id,
        [FromBody] ProjectRequest request,
        CancellationToken ct)
    {
        var updated = await _service.Update(id, request, ct);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.Delete(id, ct);
        return NoContent();
    }




}

