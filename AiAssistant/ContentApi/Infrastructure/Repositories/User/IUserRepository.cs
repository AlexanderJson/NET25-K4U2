using ContentApi.DTO;
using ContentApi.Models;

public interface IUserRepository : ICrudRepository<User>
{
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct);
}
