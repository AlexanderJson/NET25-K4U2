public interface ITopicService
{
    Task<Guid> Create(CreateTopicRequest request, CancellationToken ct);
    Task Delete(Guid Id, CancellationToken ct);
    Task Update(Guid id, UpdateTopicRequest request, CancellationToken ct);
    Task UpdateStatus(Guid id, CancellationToken ct);
}
