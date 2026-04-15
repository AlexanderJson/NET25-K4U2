using AiAssistant.ContentApi.Data;

public record CreateNotebookCommand(string Title, string Category, Guid UserId);
public class CreateNotebookHandler(AppDbContext db)
