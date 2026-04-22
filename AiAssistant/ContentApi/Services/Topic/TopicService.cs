using ContentApi.Common;
using ContentApi.Models;
using ContentApi.Services;

public class TopicService(ITopicRepository r) : ICrudService<CreateTopicRequest, UpdateTopicRequest>, ITopicService
{
    private readonly ITopicRepository _r = r;
    public async Task<Guid> Create(CreateTopicRequest request, CancellationToken ct)
    {
        ValidateInput(request);
        var topic = new Topic(request.notebookId, request.Title, request.Order);
        await _r.CreateAsync(topic, ct);
        return topic.Id;
    }

    public async Task Delete(Guid Id, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(Id);
        await _r.DeleteAsync(Id, ct);
    }

    public async Task Update(Guid id, UpdateTopicRequest request, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(id);
        var topic = await _r.GetByIdAsync(id, ct);
        Guard.Against.Null(topic);
        topic!.Rename(request.Title);
        await _r.UpdateAsync(topic, ct);
    }
    public async Task UpdateStatus(Guid id, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(id);
        var topic = await _r.GetByIdAsync(id, ct);
        Guard.Against.Null(topic);
        topic!.Complete();
        await _r.UpdateAsync(topic, ct);
    }
    private void ValidateInput(CreateTopicRequest req)
    {
        Guard.Against.NullOrWhiteSpace(req.Title);
        Guard.Against.NullOrEmptyGuid(req.notebookId);
        Guard.Against.NegativeOrZero(req.Order);
    }
}