
public sealed class LlmClient(HttpClient httpClient) : ILlmClient
{
    private readonly HttpClient _httpClient = httpClient;
    
    public async Task<string> GenerateTopicsAsync(string prompt, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync
        (
            "api/proxy",
            new GenerateTopicsProxyRequest(prompt),
            ct
        );
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(cancellationToken: ct);
        Console.WriteLine("=== FULL RAW JSON FROM PROXY ===");
        Console.WriteLine(result);
        Console.WriteLine("=== END RAW JSON ===");

        response.EnsureSuccessStatusCode();

        return result;
    }
}