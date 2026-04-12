public interface ICrudRepository<M>
{
    M Create(M model);
    M GetById(Guid id);
    void Delete(Guid id);
    M Update(M model);
    List<M> GetAll();
    

}