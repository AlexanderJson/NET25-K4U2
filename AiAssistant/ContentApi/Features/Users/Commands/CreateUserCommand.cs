using MediatR;
using ContentApi.DTO;
namespace ContentApi.Features.User.Commands;

public sealed record CreateUserCommand(
    string Username,
    string Password
) : IRequest<UserResponse>;


