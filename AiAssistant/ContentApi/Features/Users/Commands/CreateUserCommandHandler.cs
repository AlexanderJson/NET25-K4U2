using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ContentApi.Features.User.Commands;

public class CreateUserCommandHandler(AppDbContext db)
: IRequestHandler<CreateUserCommand, UserResponse>
{    
    private readonly AppDbContext _db = db;
    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var username = request.Username.Trim().ToLower();
        var exists = await _db.Users.AnyAsync(u=> u.Username == request.Username, ct);
        if(exists) throw new UsernameExistsException("Username  is taken or unavailable!");
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);        
        var user = new Persistence.Entities.User //todo
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            HashedPassword = hashedPassword,
        };

        _db.Users.Add(user);
        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            throw new UsernameExistsException("Username already exists");
        }

        return new UserResponse(user.Id, user.Username);

    }
}
