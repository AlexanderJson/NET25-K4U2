using ContentApi.Common;

namespace ContentApi.Models;

public class Topic
{
    public Guid Id { get; private set; }

    public Guid NotebookId { get; private set; }

    public Notebook Notebook { get; private set; } = default!;

    public string Title { get; private set; }

    public int Order { get; private set; }

    public bool IsCompleted { get; private set; }

    private readonly List<Note> _notes = new();

    public IReadOnlyCollection<Note> Notes => _notes;

    public DateTime CreatedAt { get; private set; }

    public byte[] RowVersion { get; private set; } = new byte[8];

    protected Topic() { }

    public Topic(Guid notebookId, string title, int order)
    {
        Guard.Against.NullOrEmptyGuid(notebookId);
        Guard.Against.NullOrWhiteSpace(title);

        Id = Guid.NewGuid();
        NotebookId = notebookId;
        Title = title.Trim();
        Order = order;
        CreatedAt = DateTime.UtcNow;
    }

    public void Rename(string title)
    {
        Guard.Against.NullOrWhiteSpace(title);
        Title = title.Trim();
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public void Reopen()
    {
        IsCompleted = false;
    }

    public void AddNote(Note note)
    {
        Guard.Against.Null(note);
        _notes.Add(note);
    }
}