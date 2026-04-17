using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;
public class UserQueries(AppDbContext db) : IUserQueries
{
    public async Task<UserResponse?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<User, UserResponse>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<UserResponse>> SearchUsersAsync(string searchTerm, CancellationToken ct = default)
    {
            return await db.Users
                        .AsNoTracking()
                        .Where(u => u.Username.Contains(searchTerm))
                        .ProjectTo<User, UserResponse>()
                        .ToListAsync(ct);
    }

 
}