namespace AiAssistant.ContentApi.Models;

public class AiGeneration
{
    public Guid Id { get; set; }

    public string Prompt { get; set; } = null!;

    public string Response { get; set; } = null!;

    public string? ModelType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
}