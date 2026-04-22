using ContentApi.Models;
using ContentApi.Projection;

public interface ITopicRepository : ICrudRepository<Topic>
{
    Task<IReadOnlyList<Topic>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken ct);
}
