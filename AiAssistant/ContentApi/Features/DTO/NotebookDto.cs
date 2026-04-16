using System.Linq.Expressions;
using ContentApi.Projection;
using ContentApi.Persistence.Entities;
namespace ContentApi.DTO;

public record NotebookRequest(string Category, string Title, Guid UserId);


public record NotebookResponse
(
    string Category,
    string Title,
    List<TopicResponse>? Topics
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




















