using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class UserRepository(AppDbContext db) : ACrudRepository<User>(db), IUserRepository
{
    public async Task<UserSummary?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<User, UserSummary>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<UserSummary>> SearchUsersAsync(string searchTerm, CancellationToken ct = default)
    {
            return await db.Users
                        .AsNoTracking()
                        .Where(u => u.Username.Contains(searchTerm))
                        .ProjectTo<User, UserSummary>()
                        .ToListAsync(ct);
    }
}
