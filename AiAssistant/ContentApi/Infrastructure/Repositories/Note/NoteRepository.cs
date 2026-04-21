using AiAssistant.ContentApi.Data;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class NoteRepository(AppDbContext db) : ACrudRepository<Note>(db), INoteRepository
{
    public async Task<NoteResponse?> GetNoteById(Guid id, CancellationToken ct)
    {
        return await _set 
            .AsNoTracking()
            .Where(n => n.Id == id)
            .ProjectTo<Note, NoteResponse>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<NoteResponse>> GetNotesByTopicId(Guid id, CancellationToken ct)
    {
        return await _set
            .AsNoTracking()
            .Where(n => n.TopicId == id)
            .ProjectTo<Note, NoteResponse>()
            .ToListAsync(ct);
    }
}
