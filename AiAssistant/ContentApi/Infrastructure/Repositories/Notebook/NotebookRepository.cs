using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Models;
using ContentApi.Projection;
using Microsoft.EntityFrameworkCore;
public class NotebookRepository(AppDbContext db) : ACrudRepository<Notebook>(db), INotebookRepository
{
    

}
