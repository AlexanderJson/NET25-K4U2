using AiAssistant.ContentApi.Data;
using Microsoft.EntityFrameworkCore;

public abstract class ACrudRepository<M>(AppDbContext db) : ICrudRepository<M> where M : class
{

    // Protected ensures  classes extending this base can still use it
    protected readonly AppDbContext _db = db;
    protected readonly DbSet<M> _set = db.Set<M>();

    
    public virtual async Task<M> CreateAsync(M model, CancellationToken ct)
    {
        await _set.AddAsync(model, ct);
        await _db.SaveChangesAsync(ct);
        return model;
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _set.FindAsync([id], ct);
        if (entity is null) return;

        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public virtual async Task<IReadOnlyList<M>> GetAllAsync(CancellationToken ct)
    {
        return await _set
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public virtual async Task<M?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _set.FindAsync([id], ct);
    }

    public virtual async Task<M> UpdateAsync(M model, CancellationToken ct)
    {
        _set.Update(model);
        await _db.SaveChangesAsync(ct);
        return model;
    }
}