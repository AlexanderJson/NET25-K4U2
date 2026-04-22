using ContentApi.Common;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Services;

public partial class NotebookService(INotebookRepository r,ITopicRepository tp, ITopicService ts) : INotebookService
{
    private readonly INotebookRepository _r = r;
    private readonly ITopicService _ts = ts;
    
    private readonly ITopicRepository _topicRepo = tp;



    public async Task<Guid> Create(CreateNotebookRequest request, CancellationToken ct)
    {
        ValidateInput(request);
        var notebook = new Notebook(request.Category, request.Title, request.UserId);
        await _r.CreateAsync(notebook,ct);
        return notebook.Id;
    }

    public async Task Delete(Guid Id, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(Id);
        await _r.DeleteAsync(Id, ct);
    }
 

    public async Task Update(Guid id, UpdateNotebookRequest request, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(id);
        Guard.Against.NullOrWhiteSpace(request.Title);
        var notebook = await _r.GetByIdAsync(id, ct);
        Guard.Against.Null(notebook);
        notebook!.UpdateTitle(request.Title!);
        await _r.UpdateAsync(notebook, ct);
    }
    public async Task AttachTopicsToBook(
        Guid notebookId,
        AttachTopicRequest request,
        CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(notebookId);
        Guard.Against.Null(request);
        Guard.Against.Null(request.TopicIds);

        var notebook = await _r.GetByIdAsync(notebookId, ct);
        Guard.Against.Null(notebook);

        var topicIds = request.TopicIds.Distinct().ToList();

        var topics = await _topicRepo.GetByIdsAsync(topicIds, ct);

        foreach (var topic in topics)
        {
            notebook!.AddTopic(topic);
        }

        await _r.UpdateAsync(notebook!, ct);
    }

    private void ValidateInput(CreateNotebookRequest req)
    {
        Guard.Against.NullOrWhiteSpace(req.Category);
        Guard.Against.NullOrWhiteSpace(req.Title);
        Guard.Against.NullOrEmptyGuid(req.UserId);
    }
}

