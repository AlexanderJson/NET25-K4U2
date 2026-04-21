using System.Linq.Expressions;
using ContentApi.Models;
using ContentApi.Projection;

public sealed record NoteResponse(string EncryptedContent, DateTime LastUpdated)
: IProjection<Note, NoteResponse>
{
    public static Expression<Func<Note, NoteResponse>> Selector 
    => n => new NoteResponse(n.EncryptedContent,  n.LastUpdated); 
}

