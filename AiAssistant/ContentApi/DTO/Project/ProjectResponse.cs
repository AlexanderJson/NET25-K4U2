using AiAssistant.ContentApi.DTO;
using AiAssistant.ContentApi.Models;

public class ProjectResponse
{

    public Guid Id {get; set;}
    public string Title {get;set;} = null!;

    public string? Description {get; set;}

    public DateTime Deadline {get; set;}

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public List<AiGenerationResponse> Stories {get; set;} = new();

}