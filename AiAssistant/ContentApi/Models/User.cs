public class User
{
    public Guid Id { get; set; }

    public required string Username { get; set; }

    public required string HashedPassword { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Notebook> Notebooks { get; set; } = new();
}