using AiAssistant.ContentApi.Data;
using ContentApi.Persistence.Entities;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class NoteQueries(AppDbContext db) : INoteQueries
{
    public async Task<NoteResponse?> GetNoteById(Guid id, CancellationToken ct)
    {
        return await db.Notes
            .AsNoTracking()
            .Where(n => n.Id == id)
            .ProjectTo<Note, NoteResponse>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<NoteResponse>> GetNotesByTopicId(Guid id, CancellationToken ct)
    {
        return await db.Notes
            .AsNoTracking()
            .Where(n => n.TopicId == id)
            .ProjectTo<Note, NoteResponse>()
            .ToListAsync(ct);
    }


}