public interface ICrudRepository<M> where M : class
{
    Task<M> CreateAsync(M model,CancellationToken ct);
    Task<M?> GetByIdAsync(Guid id,CancellationToken ct);
    Task DeleteAsync(Guid id,CancellationToken ct);
    Task<M> UpdateAsync(M model,CancellationToken ct);
    Task<IReadOnlyList<M>> GetAllAsync(CancellationToken ct);
}