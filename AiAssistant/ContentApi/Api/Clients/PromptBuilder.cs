using System.Text;
using System.Text.Json;
using ContentApi.Persistence.Entities;
using Microsoft.Extensions.Options;
public record BuildTopicPromptCommand(string title, string category);

public interface ITopicPromptBuilder
{
    string Generate(string notebookTitle, string category);
}

public sealed class TopicPromptBuilder : ITopicPromptBuilder
{
    private readonly GenerateTopicsOptions _options;

    public TopicPromptBuilder(IOptions<LlmPromptOptions> options)
    {
        _options = options.Value.GenerateTopics;
    }

    public string Generate(string notebookTitle, string category)
    {
        var sb = new StringBuilder();

        sb.AppendLine(_options.Instructions);
        sb.AppendLine();

        sb.AppendLine("Rules:");
        foreach (var rule in _options.OutputRules)
            sb.AppendLine($"- {rule}");

        sb.AppendLine("Required format:");
        sb.AppendLine(_options.OutputFormat);
        sb.AppendLine();
        sb.AppendLine("Format example:");
        sb.AppendLine(_options.OutputExample);
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine($"Notebook Title: {notebookTitle}");
        sb.AppendLine($"Category: {category}");
        sb.AppendLine();

        sb.AppendLine($"Generate {_options.MinTopics} to {_options.MaxTopics} topics.");
        sb.AppendLine();

        var prompt = sb.ToString();

        Console.WriteLine("===== GENERATED PROMPT =====");
        Console.WriteLine(prompt);
        Console.WriteLine("============================");

        return prompt;
    }
}

