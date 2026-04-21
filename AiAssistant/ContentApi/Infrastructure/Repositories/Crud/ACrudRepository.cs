using AiAssistant.ContentApi.Data;
using Microsoft.EntityFrameworkCore;

public abstract class ACrudRepository<M>(AppDbContext db) : ICrudRepository<M> where M : class
{

    // Protected ensures  classes extending this base can use the context created
    protected readonly AppDbContext _db = db; // Full context for certain operations (like if we need several tables or perform DB-wide commands like Savechanges etc )
    protected readonly DbSet<M> _set = db.Set<M>(); // this one is for all operations on the table (M)

    
    public virtual async Task CreateAsync(M model, CancellationToken ct)
    {
        await _set.AddAsync(model, ct);
        await _db.SaveChangesAsync(ct);
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _set.FindAsync([id], ct);
        if (entity is null) return;

        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }



    public virtual async Task<M?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _set.FindAsync([id], ct);
    }

    public virtual async Task UpdateAsync(M model, CancellationToken ct)
    {
        _set.Update(model);
        await _db.SaveChangesAsync(ct);
    }
    
}