public interface ICrudService<Req,Resp,Entity>
{
    Resp GetById(Guid id);
    List<Resp> GetAll();
    Resp Create(Req request);
    Resp Update(Req request);
    void Delete(Guid id);
    Entity RequestToEntity(Req r);
    Resp EntityToResponse(Entity e);

    List<Resp> EntityToResponseList(List<Entity> e);


}
