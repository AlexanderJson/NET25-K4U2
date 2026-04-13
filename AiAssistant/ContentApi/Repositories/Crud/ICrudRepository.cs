public interface ICrudRepository<M> where M : class
{
    Task<M> CreateAsync(M model);
    Task<M?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<M> UpdateAsync(M model);
    Task<IReadOnlyList<M>> GetAllAsync();
}