using AiAssistant.ContentApi.Data;
using AiAssistant.ContentApi.Models;

public class ProjectRepository(AppDbContext db) : ACrudRepository<Project>(db)
{}