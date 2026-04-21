using AiAssistant.ContentApi.Data;
using ContentApi.Models;
using Microsoft.EntityFrameworkCore;


namespace ContentApi.Projection;

public class TopicRepository(AppDbContext db) : ACrudRepository<Topic>(db), ITopicRepository
{
    public async Task<TopicSummary?> GetTopicSummaryById(Guid id, CancellationToken ct)
    {
        return await _db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Id == id)
            .ProjectTo<Topic, TopicSummary>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<TopicSummary?>> GetTopicSummariesByTitle(string title, CancellationToken ct)
    {
        return await _db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Title.Contains(title))
            .ProjectTo<Topic, TopicSummary>()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TopicSummary?>> GetTopicSummariesByCompleted(bool completed, CancellationToken ct)
    {
        return await _db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.IsCompleted == completed)
            .ProjectTo<Topic, TopicSummary>()
            .ToListAsync(ct);
    }
}