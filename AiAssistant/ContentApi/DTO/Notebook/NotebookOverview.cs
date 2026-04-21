using System.Linq.Expressions;
using ContentApi.Models;
using ContentApi.Projection;

public sealed record NotebookOverview(
    Guid Id,
    string Title,
    string Category,
    int TopicCount,
    int NoteCount,
    DateTime LastUpdated
) : IProjection<Notebook, NotebookOverview>
{
    public static Expression<Func<Notebook, NotebookOverview>> Selector =>
        n => new NotebookOverview(
            n.Id,
            n.Title,
            n.Category,
            n.Topics.Count(),
            n.Topics.SelectMany(t => t.Notes).Count(),
            n.LastUpdated
        );
}
