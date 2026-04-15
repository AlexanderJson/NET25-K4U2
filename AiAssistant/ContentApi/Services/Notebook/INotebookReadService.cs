using ContentApi.DTO;

public interface INotebookReadService
{
    Task<NotebookResponse?> GetNotebookById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<NotebookResponse>> GetNotebookByTitle(string title, CancellationToken ct);
}
