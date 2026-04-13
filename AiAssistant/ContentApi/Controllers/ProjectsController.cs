
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



}