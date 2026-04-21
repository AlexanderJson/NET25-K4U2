using ContentApi.DTO;

public interface IUserQueries
{
    Task<UserSummary?> GetUserSummaryById(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<UserSummary>> SearchUsers(string searchTerm, CancellationToken ct = default);
}
