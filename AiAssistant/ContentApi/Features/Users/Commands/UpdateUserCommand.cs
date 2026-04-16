using ContentApi.DTO;
using MediatR;
namespace ContentApi.Features.User.Commands;
public sealed record UpdateUserCommand(
    Guid Id,
    string Username,
    string? Password
) : IRequest<UserResponse>;