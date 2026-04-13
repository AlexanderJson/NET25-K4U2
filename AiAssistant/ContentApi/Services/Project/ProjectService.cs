using System.Runtime.CompilerServices;
using AiAssistant.ContentApi.Models;
using ContentApi.Common;
public class ProjectService(ProjectRepository repository) 
: IProjectService<ProjectRequest, ProjectResponse,Project>
{
    private readonly ProjectRepository _repository = repository;

    public async Task<ProjectResponse> Create(ProjectRequest request)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var created = await _repository.CreateAsync(project);
        return EntityToResponse(created);
    }

    public async Task Delete(Guid id)
    {
        Guard.Against.NullOrEmptyGuid(id);
        await _repository.DeleteAsync(id);
    }

    public async Task<IReadOnlyList<ProjectResponse>> GetAll()
    {
        var projects =  await _repository.GetAllAsync();
        return EntityToResponseList(projects);

    }

    public async Task<ProjectResponse> GetById(Guid id)
    {
        Guard.Against.NullOrEmptyGuid(id);
        var response =  await _repository.GetByIdAsync(id);
        return EntityToResponse(response!);
    }

    public async Task<ProjectResponse> Update(Guid id,ProjectRequest request)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var updated = await _repository.UpdateAsync(project);
        return EntityToResponse(updated);

    }


    public  IReadOnlyList<ProjectResponse> EntityToResponseList(IEnumerable<Project> p)
    {
        return [.. p.Select(EntityToResponse)];
    }

  

    private  void ValidateRequestArgs(ProjectRequest req)
    {
            Guard.Against.Null(req);
            Guard.Against.NullOrWhiteSpace(req.Title);
            Guard.Against.NullOrWhiteSpace(req.Description);
    }

    public  Project RequestToEntity(ProjectRequest r)
    {
        return new Project
            {
                Title = r.Title, 
                Description = r.Description, 
                Deadline = r.Deadline
            };
    }

    public  ProjectResponse EntityToResponse(Project e)
    {
        return new ProjectResponse
        {
            Id = e.Id,
            Title = e.Title, 
            Description = e.Description, 
            Deadline = e.Deadline,
            CreatedAt = e.CreatedAt
        };
    }

  
}