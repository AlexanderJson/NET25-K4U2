using System.Net.Http.Json;

public sealed class LlmProxyClient(HttpClient httpClient) : ILlmProxyClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyList<string>> GenerateTopicsAsync(string prompt, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync
        (
            "api/llm/generate-topics",
            new GenerateTopicFromLLM(prompt),
            ct
        );
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GenerateTopicsResponse>(cancellationToken: ct);
        return result?.Topics ?? [];
    }
}