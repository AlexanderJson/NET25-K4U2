using AiAssistant.ContentApi.Data;
using ContentApi.Models;

public class NoteRepository(AppDbContext db) : ACrudRepository<Note>(db), INoteRepository
{}
