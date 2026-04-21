/*


using System.Runtime.CompilerServices;
using ContentApi.Common;
using ContentApi.Models;

namespace ContentApi.Services;
public class ProjectService(ProjectRepository repository) 
: IProjectService<ProjectRequest, ProjectResponse,Project>
{
    private readonly ProjectRepository _repository = repository;

    #region CRUD methods

    public async Task<ProjectResponse> Create(ProjectRequest request, CancellationToken ct)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var created = await _repository.CreateAsync(project,ct);
        return EntityToResponse(created);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(id);
        await _repository.DeleteAsync(id,ct);
    }

    public async Task<IReadOnlyList<ProjectResponse>> GetAll( CancellationToken ct)
    {
        var projects =  await _repository.GetAllAsync(ct);
        return EntityToResponseList(projects);

    }

    public async Task<ProjectResponse> GetById(Guid id, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(id);
        var response =  await _repository.GetByIdAsync(id,ct);
        return EntityToResponse(response!);
    }

    public async Task<ProjectResponse> Update(Guid id,ProjectRequest request, CancellationToken ct)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var updated = await _repository.UpdateAsync(project,ct);
        return EntityToResponse(updated);

    }

    #endregion

    #region Mapping methods

    public  IReadOnlyList<ProjectResponse> EntityToResponseList(IEnumerable<Project> p)
    {
        return [.. p.Select(EntityToResponse)];
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
    #endregion

    #region Validation helpers
    private  void ValidateRequestArgs(ProjectRequest req)
    {
        Guard.Against.Null(req);
        Guard.Against.NullOrWhiteSpace(req.Title);
        Guard.Against.NullOrWhiteSpace(req.Description);
    }

    #endregion
    
}

*/