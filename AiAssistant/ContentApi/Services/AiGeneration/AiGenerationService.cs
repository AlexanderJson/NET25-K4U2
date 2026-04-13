using AiAssistant.ContentApi.DTO;
using AiAssistant.ContentApi.Models;
using ContentApi.Common;
public class AiGenerationService(AiGenerationRepository repository) 
: IAiGenerationService<AiGenerationRequest, AiGenerationResponse, AiGeneration>

{
    private readonly AiGenerationRepository _repository = repository;

    public async Task<AiGenerationResponse> Create(AiGenerationRequest request)
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

    public async Task<IReadOnlyList<AiGenerationResponse>> GetAll()
    {
        var projects =  await _repository.GetAllAsync();
        return EntityToResponseList(projects);
    }

    public async Task<AiGenerationResponse> GetById(Guid id)
    {
        Guard.Against.NullOrEmptyGuid(id);
        var response =  await _repository.GetByIdAsync(id);
        return EntityToResponse(response!);
    }

    public async Task<AiGenerationResponse> Update(Guid id, AiGenerationRequest request)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var updated = await _repository.UpdateAsync(project);
        return EntityToResponse(updated);
    }


    
    private void ValidateRequestArgs(AiGenerationRequest req)
    {
            Guard.Against.Null(req);
            Guard.Against.NullOrWhiteSpace(req.Prompt);
            Guard.Against.NullOrEmptyGuid(req.ProjectId);
    }

    public AiGeneration RequestToEntity(AiGenerationRequest r)
    {
        return new AiGeneration
        {
            Prompt = r.Prompt,
            ProjectId = r.ProjectId
        };
    }

    public AiGenerationResponse EntityToResponse(AiGeneration e)
    {
        return new AiGenerationResponse
        {
            Id = e.Id,
            Prompt = e.Prompt,
            Response = e.Response,
            CreatedAt = e.CreatedAt,
            ProjectId = e.ProjectId
        };
    }

    public IReadOnlyList<AiGenerationResponse> EntityToResponseList(IEnumerable<AiGeneration> e)
    {
        return [.. e.Select(EntityToResponse)];
    }
}

