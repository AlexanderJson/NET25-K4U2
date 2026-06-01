namespace LlmProxyApi.Service;

public sealed class GeminiOptions
{
    public const string SectionName = "Gemini";

    public string BaseUrl { get; init; } = string.Empty;

    public string GenerateContentEndpoint { get; init; } = string.Empty;

    public string ApiKey { get; init; } = string.Empty;

    public string ApiKeyHeaderName { get; init; } = "x-goog-api-key";

    public int TimeoutSeconds { get; init; } = 30;
}