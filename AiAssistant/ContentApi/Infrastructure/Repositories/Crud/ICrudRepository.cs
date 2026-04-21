public interface ICrudRepository<M> where M : class
{
    Task CreateAsync(M model,CancellationToken ct);
    Task<M?> GetByIdAsync(Guid id,CancellationToken ct);
    Task DeleteAsync(Guid id,CancellationToken ct);
    Task UpdateAsync(M model,CancellationToken ct);
}