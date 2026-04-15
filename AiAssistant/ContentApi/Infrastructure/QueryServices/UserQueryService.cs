using AiAssistant.ContentApi.Data;
using ContentApi.Models;
using ContentApi.Projection;

using Microsoft.EntityFrameworkCore;

public interface IUserQueryService
{
    Task<UserResponse?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<UserResponse>> SearchUsersAsync(string searchTerm, CancellationToken ct = default);
}
public class UserQueryService(AppDbContext db) : IUserQueryService
{
    public async Task<UserResponse?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectThis<User, UserResponse>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<UserResponse>> SearchUsersAsync(string searchTerm, CancellationToken ct = default)
    {
            return await db.Users
                        .AsNoTracking()
                        .Where(u => u.Username.Contains(searchTerm))
                        .ProjectThis<User, UserResponse>()
                        .ToListAsync(ct);
    }
}