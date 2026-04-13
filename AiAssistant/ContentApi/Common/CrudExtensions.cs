public static class CrudServiceExtensions
{
    public static TResponse CreateMapped<TRequest, TEntity, TResponse>(
        this ICrudRepository<TEntity> repository,
        TRequest request,
        Func<TRequest, TEntity> toEntity,
        Func<TEntity, TResponse> toResponse)
        where TEntity : class
    {
        var entity = toEntity(request);
        var created = repository.Create(entity);
        return toResponse(created);
    }

    public static TResponse GetByIdMapped<TEntity, TResponse>(
        this ICrudRepository<TEntity> repository,
        Guid id,
        Func<TEntity, TResponse> toResponse)
        where TEntity : class
    {
        var entity = repository.GetById(id);
        return toResponse(entity);
    }

    public static List<TResponse> GetAllMapped<TEntity, TResponse>(
        this ICrudRepository<TEntity> repository,
        Func<TEntity, TResponse> toResponse)
        where TEntity : class
    {
        var entities = repository.GetAll();
        return [.. entities.Select(toResponse)];
    }

    public static TResponse UpdateMapped<TRequest, TEntity, TResponse>(
        this ICrudRepository<TEntity> repository,
        TRequest request,
        Func<TRequest, TEntity> toEntity,
        Func<TEntity, TResponse> toResponse)
        where TEntity : class
    {
        var entity = toEntity(request);
        var updated = repository.Update(entity);
        return toResponse(updated);
    }

    public static void DeleteMapped<TEntity>(
        this ICrudRepository<TEntity> repository,
        Guid id)
        where TEntity : class
    {
        repository.Delete(id);
    }
}
 