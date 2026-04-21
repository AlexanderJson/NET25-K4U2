using ContentApi.Common;
using ContentApi.DTO;
using ContentApi.Models;

public class UserService(IUserRepository r) : IUserService
{
    private readonly IUserRepository _r = r;

    public async Task<Guid> Create(CreateUserRequest request, CancellationToken ct)
    {
        Guard.Against.Null(request);
        ValidateInput(request);
        var exists = await _r.UsernameExistsAsync(request.Username, ct);
        if (exists) throw new Exception("Username is taken or unavailable!");
        var hashedPassword = HashPassword(12, request.Password);

        var user = new User(request.Username, hashedPassword)
        {
            Username = request.Username,
            HashedPassword = hashedPassword
        };

        await _r.CreateAsync(user, ct);

        return user.Id;
    }

    public async Task Delete(Guid Id, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(Id);
        await _r.DeleteAsync(Id, ct);
    }


    public async Task Update(Guid id, UpdateUserRequest request, CancellationToken ct)
    {
        Guard.Against.NullOrEmptyGuid(id);
        Guard.Against.NullOrWhiteSpace(request.Username);
        var user = await _r.GetByIdAsync(id, ct);
        Guard.Against.Null(user);
        user!.UpdateUsername(request.Username!);
        await _r.UpdateAsync(user, ct);
    }

    private void ValidateInput(CreateUserRequest req)
    {
        Guard.Against.NullOrWhiteSpace(req.Username);
        Guard.Against.NullOrWhiteSpace(req.Password);
    }

    private string HashPassword(int factor, string Password)
    {
        return BCrypt.Net.BCrypt.HashPassword(Password, factor);
    }


}
