using System.Linq.Expressions;
using  ContentApi.Models;
using ContentApi.Projection;
namespace ContentApi.DTO;

// Might seem a bit redundant atm. But if we add more fields to User a summary is nice to have
public sealed record UserSummary(Guid Id, string Username): IProjection<User, UserSummary>
{
    public static Expression<Func<User, UserSummary>> Selector 
    => u => new UserSummary(u.Id, u.Username);
}


