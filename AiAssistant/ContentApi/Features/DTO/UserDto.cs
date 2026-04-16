using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ContentApi.Persistence.Entities;
using ContentApi.Projection;
namespace ContentApi.DTO;

public record UserRequest(
    [Required, MinLength(3), MaxLength(50)] string Username,
    [Required, MinLength(6)] string Password
);

public record UserResponse(Guid Id, string Username)
: IProjection<User, UserResponse>
{
    public static Expression<Func<User, UserResponse>> Selector 
    => u => new UserResponse(u.Id, u.Username);
}

public record UpdateUserRequest(
    [Required, MinLength(3), MaxLength(50)] string Username,
    [MinLength(6)] string? Password
);