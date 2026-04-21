using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;

public class UserRepository(AppDbContext db) : ACrudRepository<User>(db), IUserRepository
{
    public async Task<bool> UsernameExistsAsync(string username, CancellationToken ct)
    {
        return await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username, ct);
    }
}
