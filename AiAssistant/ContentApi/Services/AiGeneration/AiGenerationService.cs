using AiAssistant.ContentApi.DTO;
public class AiGenerationService(AiGenerationRepository repository) : IAiGenerationService<AiGenerationRequest, AiGenerationResponse>
{
    private readonly AiGenerationRepository _repository = repository;

    public AiGenerationResponse Create(AiGenerationRequest request)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<AiGenerationResponse> GetAll()
    {
        throw new NotImplementedException();
    }

    public AiGenerationResponse GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public AiGenerationResponse Update(AiGenerationRequest request)
    {
        throw new NotImplementedException();
    }
}