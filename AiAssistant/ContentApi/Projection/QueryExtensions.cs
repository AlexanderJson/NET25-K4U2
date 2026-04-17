namespace ContentApi.Projection;

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