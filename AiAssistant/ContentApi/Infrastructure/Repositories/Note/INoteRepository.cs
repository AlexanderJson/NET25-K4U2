using ContentApi.Models;

public interface INoteRepository : ICrudRepository<Note>
{
    Task<NoteResponse?> GetNoteById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<NoteResponse>> GetNotesByTopicId(Guid id, CancellationToken ct);
}
