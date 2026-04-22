using AiAssistant.ContentApi.Data;
using ContentApi.Models;
using Microsoft.EntityFrameworkCore;


namespace ContentApi.Projection;

public class TopicRepository(AppDbContext db) : ACrudRepository<Topic>(db), ITopicRepository
{
    public async Task<IReadOnlyList<Topic>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken ct)
    {
        return await _db.Topics
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(ct);
    }
}