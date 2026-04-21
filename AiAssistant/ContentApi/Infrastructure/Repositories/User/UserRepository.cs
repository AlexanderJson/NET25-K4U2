using AiAssistant.ContentApi.Data;
using ContentApi.Models;

public class UserRepository(AppDbContext db) : ACrudRepository<User>(db), IUserRepository
{}
