using AiAssistant.ContentApi.Data;
using ContentApi.Persistence.Entities;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class TopicQueries(AppDbContext db) : ITopicQueries
{
    public async Task<TopicSummary?> GetTopicById(Guid id, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Id == id)
            .ProjectTo<Topic, TopicSummary>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<TopicSummary>> GetTopicByTitle(string title, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Title.Contains(title))
            .ProjectTo<Topic, TopicSummary>()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TopicSummary>> GetTopicByCompleted(bool completed, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.IsCompleted == completed)
            .ProjectTo<Topic, TopicSummary>()
            .ToListAsync(ct);
    }
}