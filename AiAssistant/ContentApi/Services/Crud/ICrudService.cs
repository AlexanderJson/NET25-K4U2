public interface ICrudService<Req, Resp, Entity> 
where Req: class
where Resp: class
where Entity: class
{
    Task<Resp> GetById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Resp>> GetAll(CancellationToken ct);
    Task<Resp> Create(Req request, CancellationToken ct);
    Task<Resp> Update(Guid id, Req request, CancellationToken ct);
    Task Delete(Guid id, CancellationToken ct);
}