using System.Linq.Expressions;
using  ContentApi.Models;
using ContentApi.Projection;
namespace ContentApi.DTO;

//TODO - ta ut logik här?
public sealed record NotebookResponse
(
    string Category,
    string Title,
    IReadOnlyCollection<TopicResponse>? Topics
) : IProjection<Notebook, NotebookResponse>
{
    public static Expression<Func<Notebook, NotebookResponse>> Selector => 
    n => new NotebookResponse(
        n.Category, 
        n.Title, 
        n.Topics
        .AsQueryable()
        .OrderBy(o => o.Order)
        .Select(TopicResponse.Selector)
        .ToList()
    );
}




















