using AiAssistant.ContentApi.Data;
using Microsoft.EntityFrameworkCore;

public abstract class ACrudRepository<M>(AppDbContext db) : ICrudRepository<M> where M : class
{

    // Protected ensures  classes extending this base can still use it
    protected readonly AppDbContext _db = db;
    protected readonly DbSet<M> _set = db.Set<M>();

    
    public virtual async Task<M> CreateAsync(M model)
    {
        await _set.AddAsync(model);
        await _db.SaveChangesAsync();
        return model;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _set.FindAsync(id);
        if (entity is null) return;

        _set.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task<IReadOnlyList<M>> GetAllAsync()
    {
        return await _set
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<M?> GetByIdAsync(Guid id)
    {
        return await _set.FindAsync(id);
    }

    public virtual async Task<M> UpdateAsync(M model)
    {
        _set.Update(model);
        await _db.SaveChangesAsync();
        return model;
    }
}