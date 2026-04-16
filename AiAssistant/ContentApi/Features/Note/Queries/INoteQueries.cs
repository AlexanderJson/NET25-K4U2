public interface INoteQueries
{
    Task<NoteResponse?> GetNoteById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<NoteResponse>> GetNotesByTopicId(Guid id, CancellationToken ct);
}
