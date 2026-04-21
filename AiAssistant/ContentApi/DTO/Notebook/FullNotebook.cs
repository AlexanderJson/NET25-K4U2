using System.Linq.Expressions;
using ContentApi.Models;
using ContentApi.Projection;

public sealed record FullNotebook
(
    string Category,
    string Title,
    IReadOnlyList<TopicResponse>? Topics,
    IReadOnlyList<NoteResponse>? Notes
) : IProjection<Notebook, FullNotebook>
{
    public static Expression<Func<Notebook, FullNotebook>> Selector => 
    n => new FullNotebook(
        n.Category, 
        n.Title, 
            n.Topics
                .AsQueryable()
                .OrderBy(t => t.Order)
                .Select(TopicResponse.Selector)
                .ToList(),

            n.Topics
                .AsQueryable()
                .SelectMany(t => t.Notes)
                .OrderByDescending(note => note.CreatedAt)
                .Select(NoteResponse.Selector)
                .ToList()
    );
}
