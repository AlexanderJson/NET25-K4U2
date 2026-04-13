using AiAssistant.ContentApi.Data;
using AiAssistant.ContentApi.Models;

public class AiGenerationRepository(AppDbContext db) : ACrudRepository<AiGeneration>(db)
{}