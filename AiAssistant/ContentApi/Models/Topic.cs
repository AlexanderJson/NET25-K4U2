public class Topic
{
    public Guid Id { get; set; }

    public Guid NotebookId { get; set; }
    public Notebook Notebook { get; set; } = null!;

    public required string Title { get; set; }

    public int Order {get; set;} // Important since some topics are precursor to others etc
    
    public bool IsCompleted {get; set;} // user can check completed to signal that they are done with that part of the study

    public List<string> Tags { get; set; } = new();

    public List<Note> Notes { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}