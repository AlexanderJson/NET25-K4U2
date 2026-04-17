using System.Linq.Expressions;
using ContentApi.Persistence.Entities;
using ContentApi.Projection;

public record CreateTopicRequest(Guid NotebookId, string Title, int Order);
public sealed record TopicSummary(string Title, int Order, bool IsCompleted) : IProjection<Topic, TopicSummary>
    {
        public static Expression<Func<Topic, TopicSummary>> Selector =>
            t => new TopicSummary
            (
                t.Title,
                t.Order,
                t.IsCompleted
            );
    }

public record TopicWithNotesResponse(Guid Id, string Title, int Order, bool IsCompleted, List<NoteResponse> Notes)
: IProjection<Topic, TopicWithNotesResponse>
{
    public static Expression<Func<Topic, TopicWithNotesResponse>> Selector 
    => t => new TopicWithNotesResponse
    (
        t.Id,
        t.Title, 
        t.Order, 
        t.IsCompleted, 
        t.Notes.AsQueryable().Select(NoteResponse.Selector).ToList()
    );
}

public sealed record GenerateTopicsProxyResponse(IReadOnlyList<string> Topics);
public sealed record GenerateTopicsProxyRequest(string Prompt);
public sealed record GenerateTopicsRequest(Guid NotebookId);

public class GeminiRequest
{
    public string Prompt { get; set; } = string.Empty;
}

public class GeminiResponse
{
    public string Content { get; set; } = string.Empty;
}

