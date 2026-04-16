namespace ContentApi.Persistence.Entities;

public class Notebook
{
    public Guid Id { get; set; }

    public required string Category { get; set; }

    public required string Title { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string? HashedPassword { get; set; }

    public List<Topic> Topics { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public byte[] RowVersion { get; set; } = default!;
}