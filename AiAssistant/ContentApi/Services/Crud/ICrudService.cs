public interface ICrudService<Req,Resp>
{
    Resp GetById(Guid id);
    List<Resp> GetAll();
    Resp Create(Req request);
    Resp Update(Req request);
    void Delete(Guid id);

}
