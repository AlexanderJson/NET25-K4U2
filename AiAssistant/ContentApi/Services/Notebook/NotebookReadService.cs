using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class NotebookQueries(AppDbContext db) : INotebookReadService
{
    public async Task<NotebookResponse?> GetNotebookById(Guid id, CancellationToken ct)
    {
        return await db.Notebooks
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Id == id)
            .ProjectTo<Notebook, NotebookResponse>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<NotebookResponse>> GetNotebookByTitle(string title, CancellationToken ct)
    {
        return await db.Notebooks
            .AsNoTracking()
            .AsSplitQuery()
            .Where(n => n.Title.Contains(title))
            .ProjectTo<Notebook, NotebookResponse>()
            .ToListAsync(ct);
    }
}