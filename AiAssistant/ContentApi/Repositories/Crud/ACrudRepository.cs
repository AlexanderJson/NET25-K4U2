using AiAssistant.ContentApi.Data;
using Microsoft.EntityFrameworkCore;

public abstract class ACrudRepository<M>(AppDbContext db) : ICrudRepository<M> where M : class
{

    // Protected ensures  classes extending this base can still use it
    protected readonly AppDbContext _db = db;
    protected readonly DbSet<M> _set = db.Set<M>();

    

    public virtual M Create(M model)
    {
        _set.Add(model);
        _db.SaveChanges();
        return model;
    }

    public virtual void Delete(Guid id)
    {
        var e = _set.Find(id);
        if(e!= null)
        {
            _set.Remove(e);
            _db.SaveChanges();
        }
    }

    public virtual List<M> GetAll()
    {
        return [.. _set];
    }

    public virtual M GetById(Guid id)
    {
        return _set.Find(id)!;
    }

    public virtual M Update(M model)
    {
        _set.Update(model);
        _db.SaveChanges();
        return model;
    }
}