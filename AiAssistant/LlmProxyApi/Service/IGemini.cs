public interface IGeminiService
{
        Task<IReadOnlyList<string>> GenerateTopicsAsync(string prompt, CancellationToken ct);

}