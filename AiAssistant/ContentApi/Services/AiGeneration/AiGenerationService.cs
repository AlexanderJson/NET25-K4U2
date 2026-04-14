using AiAssistant.ContentApi.DTO;
using ContentApi.Common;
using ContentApi.Models;
namespace ContentApi.Services;

public class AiGenerationService(AiGenerationRepository repository) 
: IAiGenerationService<AiGenerationRequest, AiGenerationResponse, AiGeneration>

{
    private readonly AiGenerationRepository _repository = repository;

    #region CRUD methods

    public async Task<AiGenerationResponse> Create(AiGenerationRequest request, CancellationToken ct)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var created = await _repository.CreateAsync(project, ct);
        return EntityToResponse(created);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(id);
        await _repository.DeleteAsync(id, ct);
    }

    public async Task<IReadOnlyList<AiGenerationResponse>> GetAll(CancellationToken ct)
    {
        var projects =  await _repository.GetAllAsync(ct);
        return EntityToResponseList(projects);
    }

    public async Task<AiGenerationResponse> GetById(Guid id, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(id);
        var response =  await _repository.GetByIdAsync(id, ct);
        return EntityToResponse(response!);
    }

    public async Task<AiGenerationResponse> Update(Guid id, AiGenerationRequest request, CancellationToken ct)
    {
        ValidateRequestArgs(request);
        var project = RequestToEntity(request);
        var updated = await _repository.UpdateAsync(project, ct);
        return EntityToResponse(updated);
    }

    #endregion
    #region Mapping methods
    
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
    #endregion
    #region Validation Methods

    private void ValidateRequestArgs(AiGenerationRequest req)
    {
        Guard.Against.Null(req);
        Guard.Against.NullOrWhiteSpace(req.Prompt);
        Guard.Against.NullOrEmptyGuid(req.ProjectId);
    }

    #endregion
}