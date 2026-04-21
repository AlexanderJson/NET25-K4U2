using AiAssistant.ContentApi.Data;
using ContentApi.Models;

public class TopicRepository(AppDbContext db) : ACrudRepository<Topic>(db), ITopicRepository
{}