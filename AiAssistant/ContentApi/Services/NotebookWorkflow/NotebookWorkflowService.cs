using System.Text.Json;
using ContentApi.Common;
using ContentApi.Models;

public class NotebookWorkflowService
(
    INotebookRepository notebooks, 
    ITopicRepository topicRepo, 
    //INoteRepository notes, 
    ILlmClient llm,
    ITopicPromptBuilder prompts
)
{   
    private readonly ITopicRepository _topicRepo = topicRepo;

    private readonly INotebookRepository _nbRepo = notebooks;
    private readonly ILlmClient _llm = llm;
    private readonly ITopicPromptBuilder _prompt = prompts;

    public async Task<IReadOnlyList<Guid>> GenerateTopicsFromGemini(Guid notebookId, CancellationToken ct)
    {
        var notebook = await _nbRepo.GetByIdAsync(notebookId,ct);
        Guard.Against.Null(notebook);
        var prompt = _prompt.Generate(notebook!.Title,notebook.Category);
        var raw = await _llm.GenerateTopicsAsync(prompt, ct);
        var topics = ParseTopics(raw);
        var order = 1;
        var createdIds = new List<Guid>();

        foreach(var title in topics)
        {
            var topic = new Topic(notebook.Id, title, order++);
            await _topicRepo.CreateAsync(topic, ct);
            createdIds.Add(topic.Id);
        }
        return createdIds;
    }

    private static IReadOnlyList<string> ParseTopics(string json)
    {
        using var outer = JsonDocument.Parse(json);

        var innerJson = outer.RootElement
            .GetProperty("result")
            .GetString();

        if (string.IsNullOrWhiteSpace(innerJson))
            throw new InvalidOperationException("Proxy returned empty result.");

        using var doc = JsonDocument.Parse(innerJson);

        var topics = doc.RootElement
            .EnumerateArray()
            .Select(x => x.GetString()?.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Cast<string>()
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (topics.Count == 0)
            throw new InvalidOperationException("No topics returned.");

        return topics;
    }
}