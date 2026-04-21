using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;
public sealed class NotebookQueries(AppDbContext context) : INotebookQueries
{
    public async Task<NotebookResponse?> GetNotebookById(Guid id,CancellationToken ct)
    {
        return await context.Notebooks
            .AsNoTracking()
            .Where(n => n.Id == id)
            .ProjectTo<Notebook, NotebookResponse>()
            .FirstOrDefaultAsync(ct);
    }
    public async Task<IReadOnlyList<NotebookResponse>> GetNotebookByTitle(string title,CancellationToken ct)
    {
        return await context.Notebooks
            .AsNoTracking()
            .Where(n => n.Title.Contains(title))
            .ProjectTo<Notebook, NotebookResponse>()
            .ToListAsync(ct);
    }
    public async Task<IReadOnlyList<NotebookOverview>> GetNotebookOverview(Guid userId,CancellationToken ct)
    {
        return await context.Notebooks
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .ProjectTo<Notebook, NotebookOverview>()
            .ToListAsync(ct);
    }
    public async Task<FullNotebook?> GetFullNotebookById(Guid NotebookId, CancellationToken ct)
    {
        return await context.Notebooks
            .AsNoTracking()
            .Where(n => n.Id == NotebookId)
            .ProjectTo<Notebook, FullNotebook>()
            .FirstOrDefaultAsync(ct);
    }

}