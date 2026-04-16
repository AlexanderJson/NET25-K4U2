using MediatR;
namespace ContentApi.Features.User.Commands;

public sealed record DeleteUserCommand(Guid Id) : IRequest;