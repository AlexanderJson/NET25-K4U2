using MediatR;

public sealed record GenerateTopicsFromLlmCommand(Guid NotebookId) : IRequest<IReadOnlyList<string>>;