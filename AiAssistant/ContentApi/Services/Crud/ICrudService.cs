namespace ContentApi.Services;

public interface ICrudService<TCreate,TUpdate> 
where TCreate: class
where TUpdate: class
{
    Task<Guid> Create(TCreate request, CancellationToken ct);
    Task Update(TUpdate request, CancellationToken ct);
    Task Delete(Guid id, CancellationToken ct);
}