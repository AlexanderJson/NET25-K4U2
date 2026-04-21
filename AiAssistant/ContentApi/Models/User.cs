using ContentApi.Common;

namespace ContentApi.Models;

public class User
{
    public Guid Id { get; set; }

    public required string Username { get; set; }

    public required string HashedPassword { get; set; }

    public List<Notebook> Notebooks { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    protected User() { }

    public User(string username, string hashedPassword)
    {
        Guard.Against.NullOrWhiteSpace(username);
        Guard.Against.NullOrWhiteSpace(hashedPassword);

        Id = Guid.NewGuid();
        Username = username.Trim();
        HashedPassword = hashedPassword;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateUsername(string username)
    {
        Guard.Against.NullOrWhiteSpace(username);

        Username = username.Trim();
    }

    public void UpdatePassword(string hashedPassword)
    {
        Guard.Against.NullOrWhiteSpace(hashedPassword);

        HashedPassword = hashedPassword;
    }
}