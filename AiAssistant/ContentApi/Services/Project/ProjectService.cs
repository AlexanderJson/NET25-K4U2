using AiAssistant.ContentApi.Models;
public class ProjectService(ProjectRepository repository) : IProjectService<ProjectRequest, ProjectResponse>
{
    private readonly ProjectRepository _repository = repository;

    public ProjectResponse Create(ProjectRequest request)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<ProjectResponse> GetAll()
    {
        throw new NotImplementedException();
    }

    public ProjectResponse GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public ProjectResponse Update(ProjectRequest request)
    {
        throw new NotImplementedException();
    }
}