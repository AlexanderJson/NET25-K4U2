using System.Linq.Expressions;
using ContentApi.Persistence.Entities;
using ContentApi.Projection;

public record TopicRequest(Guid NotebookId, string Title, int Order);
public sealed record NotebookTopicsSummary(string Title, int Order, bool IsCompleted) : IProjection<Topic, NotebookTopicsSummary>
    {
        public static Expression<Func<Topic, NotebookTopicsSummary>> Selector =>
            t => new NotebookTopicsSummary
            (
                t.Title,
                t.Order,
                t.IsCompleted
            );
    }


public record TopicResponse(string Title, int Order, bool IsCompleted, List<NoteResponse> Notes)
: IProjection<Topic, TopicResponse>
{
    public static Expression<Func<Topic, TopicResponse>> Selector 
    => t => new TopicResponse
    (
        t.Title, 
        t.Order, 
        t.IsCompleted, 
        t.Notes.AsQueryable().Select(NoteResponse.Selector).ToList()
    );
}
