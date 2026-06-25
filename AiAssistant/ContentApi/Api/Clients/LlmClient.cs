

using System.Text.RegularExpressions;

public sealed class LlmClient(HttpClient httpClient) : ILlmClient
{
    private readonly HttpClient _httpClient = httpClient;

//todo replace with smarter solutino later!
    private static string RedactSnippet(string? content, int maxLength)
    {
        if (string.IsNullOrEmpty(content))
            return "[no content]";

        // Truncates the text snippet  first
        var snippet = content.Length > maxLength ? content.Substring(0, maxLength) : content;

        try
        {
            // pasttern we use 
            var pattern = "(?i)(api[_-]?key|authorization|bearer)\\s*[:=]\\s*[^\\s,\"']+";
            snippet = Regex.Replace(snippet, pattern, "[REDACTED]");
        }
        catch
        {
            // just fall back if regex fails
        }

        return snippet;
    }

    public async Task<string> GenerateTopicsAsync(string prompt, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/proxy",
                new GenerateTopicsProxyRequest(prompt),
                ct
            );

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken: ct);
                var snippet = RedactSnippet(content, 300);
                var headers = new Dictionary<string, string?>();
                foreach (var h in response.Headers)
                    headers[h.Key] = string.Join(",", h.Value);
                throw new ContentApi.Common.ExternalApiException((int)response.StatusCode, snippet, headers);
            }

            var result = await response.Content.ReadAsStringAsync(cancellationToken: ct);
            if (string.IsNullOrWhiteSpace(result))
                throw new HttpRequestException("Empty response from proxy service.");

            return result;
        }
        catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
        {
            throw new TaskCanceledException("Request to proxy timed out.", ex);
        }
    }
}