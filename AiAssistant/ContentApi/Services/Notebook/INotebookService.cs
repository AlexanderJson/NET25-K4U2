namespace ContentApi.Services;

public interface INotebookService<Req,Resp,Entity> : ICrudService<Req, Resp,Entity>
where Req: class
where Resp: class
where Entity: class
{}
