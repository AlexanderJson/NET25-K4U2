using ContentApi.Models;
using System.Linq.Expressions;
namespace ContentApi.Projection;

public sealed record TopicSummary(
    Guid Id,
    string Title,
    int Order,
    bool IsCompleted
) : IProjection<Topic, TopicSummary>
{
    public static Expression<Func<Topic, TopicSummary>> Selector =>
        topic => new TopicSummary(
            topic.Id,
            topic.Title,
            topic.Order,
            topic.IsCompleted
        );
}
