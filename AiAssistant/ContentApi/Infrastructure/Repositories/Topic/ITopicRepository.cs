using ContentApi.Models;
using ContentApi.Projection;

public interface ITopicRepository : ICrudRepository<Topic>
{
    Task<IReadOnlyList<TopicSummary?>> GetTopicSummariesByCompleted(bool completed, CancellationToken ct);
    Task<TopicSummary?> GetTopicSummaryById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<TopicSummary?>> GetTopicSummariesByTitle(string title, CancellationToken ct);
}
