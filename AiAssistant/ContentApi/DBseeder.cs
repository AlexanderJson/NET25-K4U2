using AiAssistant.ContentApi.Data;
using ContentApi.Persistence.Entities;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Users.Any())
            return;  

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "alex",
            HashedPassword = "dev"
        };

        var notebook1 = new Notebook
        {
            Id = Guid.NewGuid(),
            Title = "Learn C#",
            Category = "Backend",
            User = user,
            RowVersion = [0]
        };

        var notebook2 = new Notebook
        {
            Id = Guid.NewGuid(),
            Title = "Learn AI",
            Category = "Machine Learning",
            User = user,
                        RowVersion = [0]

        };



        db.Users.Add(user);
        db.Notebooks.AddRange(notebook1, notebook2);

        await db.SaveChangesAsync();
                var notebooks = new[] { notebook1, notebook2 };
        foreach (var notebook in notebooks)
        {

            Console.WriteLine();
            Console.WriteLine($"Notebook ID: {notebook.Id}");
            Console.WriteLine($"Title: {notebook.Title}");
            Console.WriteLine($"Category: {notebook.Category}");
            Console.WriteLine($"UserId: {user.Id}");

 
        }
    }
}