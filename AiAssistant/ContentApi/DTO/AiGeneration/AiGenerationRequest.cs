namespace AiAssistant.ContentApi.DTO;

public class AiGenerationRequest
{
    public string Prompt { get; set; } = null!;


    public Guid? ProjectId { get; set; }
}