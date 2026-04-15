using System.Linq.Expressions;
using System.Xml;
using  ContentApi.Models;
using ContentApi.Projection;
namespace ContentApi.DTO;
public record CreateNotebook
(
    string Category, 
    string Title, 
    Guid UserId
);

public record NotebookSummary
(
    string Category, 
    string Title, 
    Guid UserId, 
    DateTime CreatedAt, 
    DateTime LastUpdated, 
    List<Topic>? Topics
)
: IProjection<Notebook, NotebookSummary>
{
    public static Expression<Func<Notebook, NotebookSummary>> Selector 
    => n => new NotebookSummary(n.Category, n.Title, n.UserId, n.CreatedAt, n.LastUpdated, n.Topics);
}