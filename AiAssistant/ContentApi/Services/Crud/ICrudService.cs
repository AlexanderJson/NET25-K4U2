public interface ICrudService<Req, Resp, Entity> 
where Req: class
where Resp: class
where Entity: class
{
    Task<Resp> GetById(Guid id);
    Task<IReadOnlyList<Resp>> GetAll();
    Task<Resp> Create(Req request);
    Task<Resp> Update(Guid id, Req request);
    Task Delete(Guid id);
}