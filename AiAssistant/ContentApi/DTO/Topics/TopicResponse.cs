using System.Linq.Expressions;
using ContentApi.Models;
using ContentApi.Projection;

public sealed record TopicResponse(string Title, int Order, bool IsCompleted, List<NoteResponse> Notes)
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
