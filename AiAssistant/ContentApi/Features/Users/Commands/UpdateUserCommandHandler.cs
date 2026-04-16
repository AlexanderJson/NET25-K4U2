using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ContentApi.Features.User.Commands;

public class UpdateUserCommandHandler(AppDbContext db)
: IRequestHandler<UpdateUserCommand, UserResponse>
{    
    private readonly AppDbContext _db = db;
    public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var username = request.Username.Trim().ToLower();
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct) ?? throw new UserNotFoundException($"User with id '{request.Id}' was not found.");
        
        var exists = await _db.Users.AnyAsync(u=> u.Id != request.Id && u.Username == request.Username, ct);
        if(exists) throw new UsernameExistsException("Username is taken or unavailable!");

        user.Username = username;
                if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(
                request.Password,
                workFactor: 12);
        }

        await _db.SaveChangesAsync(ct);

        return new UserResponse(user.Id, user.Username);


    }
}
