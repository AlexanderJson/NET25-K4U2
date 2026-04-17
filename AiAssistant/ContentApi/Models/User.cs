namespace ContentApi.Models;

public class User
{
    public Guid Id { get; set; }

    public required string Username { get; set; }

    public required string HashedPassword { get; set; }

    public List<Notebook> Notebooks { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}