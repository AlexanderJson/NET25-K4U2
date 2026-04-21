using ContentApi.DTO;
using ContentApi.Models;

public interface IUserRepository : ICrudRepository<User>
{
    Task<UserSummary?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<UserSummary>> SearchUsersAsync(string searchTerm, CancellationToken ct = default);
}
