public interface ILlmProxyClient
{
    Task<IReadOnlyList<string>> GenerateTopicsAsync(string prompt, CancellationToken ct);
}