using ContentApi.Common;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Services;

public class NotebookService(INotebookRepository r) 
: INotebookService<CreateNotebookRequest, UpdateNotebookRequest>
{
    private readonly INotebookRepository _r = r;

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
 

    public async Task Update(UpdateNotebookRequest request, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(request.Id);
        Guard.Against.NullOrWhiteSpace(request.Title);
        var notebook = await _r.GetByIdAsync(request.Id, ct);
        Guard.Against.Null(notebook);
        notebook!.UpdateTitle(request.Title!);
        await _r.UpdateAsync(notebook, ct);
    }

    private void ValidateInput(CreateNotebookRequest req)
    {
        Guard.Against.NullOrWhiteSpace(req.Category);
        Guard.Against.NullOrWhiteSpace(req.Title);
        Guard.Against.NullOrEmptyGuid(req.UserId);
    }
}

