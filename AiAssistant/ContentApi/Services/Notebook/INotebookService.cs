using ContentApi.DTO;

namespace ContentApi.Services;

public interface INotebookService : ICrudService<CreateNotebookRequest, UpdateNotebookRequest>
{
    Task AttachTopicsToBook(Guid notebookId,AttachTopicRequest request,CancellationToken ct);
}
