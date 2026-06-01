namespace ContentApi.DTO;

public sealed class LlmProxyOptions
{
    public const string SectionName = "LlmProxy";

    public string BaseUrl { get; init; } = string.Empty;

    public int TimeoutSeconds { get; init; } = 30;
}