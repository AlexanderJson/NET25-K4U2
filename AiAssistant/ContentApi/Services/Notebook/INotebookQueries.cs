using ContentApi.DTO;
using ContentApi.Models;

public interface INotebookQueries 
{
    Task<FullNotebook?> GetFullNotebookById(Guid NotebookId, CancellationToken ct);
    Task<NotebookResponse?> GetNotebookById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<NotebookResponse>> GetNotebookByTitle(string title, CancellationToken ct);
    Task<IReadOnlyList<NotebookOverview>> GetNotebookOverview(Guid userId, CancellationToken ct);
} 
