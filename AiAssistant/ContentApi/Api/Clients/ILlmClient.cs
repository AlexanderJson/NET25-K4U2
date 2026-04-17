public interface ILlmClient
{
    Task<string> GenerateTopicsAsync(string prompt, CancellationToken ct);
}