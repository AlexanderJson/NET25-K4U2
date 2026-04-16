using ContentApi.DTO;

public interface IUserQueries
{
    Task<UserResponse?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<UserResponse>> SearchUsersAsync(string searchTerm, CancellationToken ct = default);
}
