using AiAssistant.ContentApi.Data;
using ContentApi.Models;

public class AiGenerationRepository(AppDbContext db) : ACrudRepository<AiGeneration>(db)
{}