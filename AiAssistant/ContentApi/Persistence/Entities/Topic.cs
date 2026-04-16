namespace ContentApi.Persistence.Entities;

public class Topic
{
    public Guid Id { get; set; }

    public Guid NotebookId { get; set; }
    public Notebook Notebook { get; set; } = null!;

    public required string Title { get; set; }

    public int Order { get; set; }

    public bool IsCompleted { get; set; }

    public List<Note> Notes { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public byte[] RowVersion { get; set; } = default!;
}