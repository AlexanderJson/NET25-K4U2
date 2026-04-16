using AiAssistant.ContentApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ContentApi.Features.User.Commands;

public sealed class DeleteUserCommandHandler(AppDbContext db)
    : IRequestHandler<DeleteUserCommand>
{
    private readonly AppDbContext _db = db;

    public async Task Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

        if (user is null)
            throw new UserNotFoundException($"User with id '{request.Id}' was not found.");

        _db.Users.Remove(user);

        await _db.SaveChangesAsync(ct);
    }
}