namespace ContentApi.Services;

public interface INotebookService<TCreate, TUpdate> : ICrudService<TCreate, TUpdate>
where TCreate: class
where TUpdate: class
{
    
}
