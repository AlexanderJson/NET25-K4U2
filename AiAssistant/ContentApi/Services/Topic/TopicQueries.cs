using AiAssistant.ContentApi.Data;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class TopicQueries(AppDbContext db) : ITopicQueries
{
    public async Task<TopicSummary?> GetTopicSummaryById(Guid id, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Id == id)
            .ProjectTo<Topic, TopicSummary>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<TopicSummary?>> GetTopicSummariesByTitle(string title, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Title.Contains(title))
            .ProjectTo<Topic, TopicSummary>()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TopicSummary?>> GetTopicSummariesByCompleted(bool completed, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.IsCompleted == completed)
            .ProjectTo<Topic, TopicSummary>()
            .ToListAsync(ct);
    }
}