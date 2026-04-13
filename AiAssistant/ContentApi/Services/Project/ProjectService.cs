using AiAssistant.ContentApi.Models;
using ContentApi.Common;
public class ProjectService(ProjectRepository repository) : IProjectService<ProjectRequest, ProjectResponse,Project>
{
    private readonly ProjectRepository _repository = repository;

    public ProjectResponse Create(ProjectRequest request)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var created = _repository.Create(project);
        return EntityToResponse(created);
    }

    public void Delete(Guid id)
    {
        Guard.Against.NullOrEmptyGuid(id);
        _repository.Delete(id);
    }

    public List<ProjectResponse> GetAll()
    {
        var projects =  _repository.GetAll();
        return EntityToResponseList(projects);

    }

    public ProjectResponse GetById(Guid id)
    {
        Guard.Against.NullOrEmptyGuid(id);
        var response =  _repository.GetById(id);
        return EntityToResponse(response);
    }

    public ProjectResponse Update(ProjectRequest request)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var updated = _repository.Update(project);
        return EntityToResponse(updated);

    }


    public List<ProjectResponse> EntityToResponseList(List<Project> p)
    {
        return [.. p.Select(EntityToResponse)];
    }

  

    private void ValidateRequestArgs(ProjectRequest req)
    {
            Guard.Against.Null(req);
            Guard.Against.NullOrWhiteSpace(req.Title);
            Guard.Against.NullOrWhiteSpace(req.Description);
    }

    public Project RequestToEntity(ProjectRequest r)
    {
        return new Project
            {
                Title = r.Title, 
                Description = r.Description, 
                Deadline = r.Deadline
            };
    }

    public ProjectResponse EntityToResponse(Project e)
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