public interface ITopicQueries
{
    Task<IReadOnlyList<TopicResponse>> GetTopicByCompleted(bool completed, CancellationToken ct);
    Task<TopicResponse?> GetTopicById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<TopicResponse>> GetTopicByTitle(string title, CancellationToken ct);
}
