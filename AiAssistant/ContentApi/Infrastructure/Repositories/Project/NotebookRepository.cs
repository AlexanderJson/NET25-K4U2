using AiAssistant.ContentApi.Data;
using ContentApi.Models;
public class NotebookRepository(AppDbContext db) : ACrudRepository<Project>(db)
{
    
}