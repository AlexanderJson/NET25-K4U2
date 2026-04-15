using AiAssistant.ContentApi.Data;
using ContentApi.Models;
public class ProjectRepository(AppDbContext db) : ACrudRepository<Project>(db)
{
    
}