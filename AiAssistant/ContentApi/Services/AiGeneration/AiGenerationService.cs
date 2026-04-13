using AiAssistant.ContentApi.DTO;
using AiAssistant.ContentApi.Models;
using ContentApi.Common;
public class AiGenerationService(AiGenerationRepository repository) 
: IAiGenerationService<AiGenerationRequest, AiGenerationResponse, AiGeneration>
{
    private readonly AiGenerationRepository _repository = repository;

    public AiGenerationResponse Create(AiGenerationRequest request)
    {
        ValidateRequestArgs(request);
        var AiGen = RequestToEntity(request);
        var created = _repository.Create(AiGen);
        return EntityToResponse(created);
    }

    public void Delete(Guid id)
    {
        Guard.Against.NullOrEmptyGuid(id);
        _repository.Delete(id);
    }

    public List<AiGenerationResponse> GetAll()
    {
        var response = _repository.GetAll();
        return EntityToResponseList(response);
    }

    public AiGenerationResponse GetById(Guid id)
    {
        Guard.Against.NullOrEmptyGuid(id);
        var response = _repository.GetById(id);
        return EntityToResponse(response);
    }

    public AiGenerationResponse Update(AiGenerationRequest request)
    {
        ValidateRequestArgs(request);
        var entity = RequestToEntity(request);
        var response = _repository.Update(entity);
        return EntityToResponse(response);

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

    public List<AiGenerationResponse> EntityToResponseList(List<AiGeneration> e)
    {
        return [.. e.Select(EntityToResponse)];
    }
}

