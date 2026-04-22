using ContentApi.Projection;

public interface ITopicQueries
{
    Task<IReadOnlyList<TopicSummary?>> GetTopicSummariesByCompleted(bool completed, CancellationToken ct);
    Task<IReadOnlyList<TopicSummary?>> GetTopicSummariesByTitle(string title, CancellationToken ct);
    Task<TopicSummary?> GetTopicSummaryById(Guid id, CancellationToken ct);
}
