using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class UserQueries(AppDbContext context) : IUserQueries
{
    public async Task<UserSummary?> GetUserSummaryById(Guid id, CancellationToken ct = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<User, UserSummary>()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<UserSummary>> SearchUsers(string searchTerm, CancellationToken ct = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Username.Contains(searchTerm))
            .ProjectTo<User, UserSummary>()
            .ToListAsync(ct);
    }
}