
using AiAssistant.ContentApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v0/[controller]")]
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
    /// Tries to create fetch all projects
    /// </summary>
    /// <param name="ct">This is a struct that flags if a process should be stopped early. 
    /// F.example if the TCP connection is closed, a timeout or if user closes the browser.</param>
    /// <returns></returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>>GetAll(CancellationToken ct)
    {
        var result = await _service.GetAll(ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>>GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetById(id,ct);
        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>>Create([FromBody] ProjectRequest request, CancellationToken ct)
    {
        var result = await _service.Create(request,ct);
        return Ok(result);
    }


}

