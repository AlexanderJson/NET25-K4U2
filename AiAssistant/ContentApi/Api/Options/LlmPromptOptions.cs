public sealed class LlmPromptOptions
{
    public GenerateTopicsOptions GenerateTopics { get; set; } = new();
}

public sealed class GenerateTopicsOptions
{
    public string Instructions { get; set; } = "";
    public List<string> OutputRules { get; set; } = [];
    public string OutputFormat { get; set; } = default!;
    public string OutputExample { get; set; } = default!;
    public int MinTopics { get; set; }
    public int MaxTopics { get; set; }
}