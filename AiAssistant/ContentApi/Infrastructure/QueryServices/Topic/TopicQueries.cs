using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class TopicQueries(AppDbContext db) : ITopicQueries
{
    public async Task<TopicResponse?> GetTopicById(Guid id, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Id == id)
            .ProjectTo<Topic, TopicResponse>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<TopicResponse>> GetTopicByTitle(string title, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Title.Contains(title))
            .ProjectTo<Topic, TopicResponse>()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TopicResponse>> GetTopicByCompleted(bool completed, CancellationToken ct)
    {
        return await db.Topics
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.IsCompleted == completed)
            .ProjectTo<Topic, TopicResponse>()
            .ToListAsync(ct);
    }
}