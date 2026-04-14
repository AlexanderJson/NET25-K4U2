
public class Note
{
    public Guid Id { get; set; }

    public Guid TopicId { get; set; }
    public Topic Topic { get; set; } = null!;

    public required string EncryptedContent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

}