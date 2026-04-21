namespace ContentApi.Projection;

/// <summary>
/// Takes one query(entity) and projects to another query(dto/record)
/// This is used in the context where we need to convert entity into dto.
/// Mainly for reusability
/// </summary>
public static class QueryExtensions
{
    public static IQueryable<Dto> ProjectTo<Entity, Dto>
    (this IQueryable<Entity> query)
    where Entity: class
    where Dto: class, IProjection<Entity,Dto>
    {
        return query.Select(Dto.Selector);
    }
}