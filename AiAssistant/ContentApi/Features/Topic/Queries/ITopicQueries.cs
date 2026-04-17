public interface ITopicQueries
{
    Task<IReadOnlyList<TopicSummary>> GetTopicByCompleted(bool completed, CancellationToken ct);
    Task<TopicSummary?> GetTopicById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<TopicSummary>> GetTopicByTitle(string title, CancellationToken ct);
}
