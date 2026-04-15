using System.Linq.Expressions;
using  ContentApi.Models;
using ContentApi.Projection;
namespace ContentApi.DTO;
public record CreateUserDto(string Username, string Password);

public record UserResponse(string Username, DateTime CreatedAt)
: IProjection<User, UserResponse>
{
    public static Expression<Func<User, UserResponse>> Selector 
    => u => new UserResponse(u.Username, u.CreatedAt);
}