using ContentApi.Common;

namespace ContentApi.Models;

public class Notebook
{
    public Guid Id { get; private set; }
    public const int MaxTitleLength = 80;
    public const int MaxCategoryLength = 80;
    public  string Category { get; private set; }

    public  string Title { get; private set; }

    public Guid UserId { get; private set; }

    public User? User {get; private set;}

    // This is my mutable list (still private)
    private readonly List<Topic> _topics = new();
    // and this is the immutable version derived from the mutable
    public IReadOnlyCollection<Topic> Topics => _topics;

    // these three are handled by db triggers  
    public DateTime CreatedAt { get; private set; } 

    public DateTime LastUpdated { get; private set; }  

    public byte[] RowVersion { get; private set; } = new byte[8];
    protected Notebook(){}
    public Notebook(string category, string title, Guid userId)
    {
       Guard.Against.NullOrWhiteSpace(title);
       Guard.Against.NullOrWhiteSpace(category);
       Guard.Against.NullOrEmptyGuid(userId);

        Id = Guid.NewGuid();
        category = category.Trim();
        title = title.Trim();
        CheckCharLength(title, MaxTitleLength);
        CheckCharLength(category, MaxCategoryLength);
        Category = category;
        Title = title;
        UserId = userId;
    }

    public void UpdateTitle(string title)
    {
        Guard.Against.NullOrWhiteSpace(title);
        title = title.Trim();
        CheckCharLength(title, MaxTitleLength);
        Title = title;
    }


    public void AddTopic(Topic topic)
    {
        Guard.Against.Null(topic);
        //var normInput = topic.Title.Trim().ToUpperInvariant();
        //if(_topics.Any(t => t.Title.Trim().Equals(normInput, StringComparison.OrdinalIgnoreCase))) throw new InvalidOperationException("This topic already exists!");
        _topics.Add(topic);
    }

    private void CheckCharLength(string input, int maxLength)
    {
        if (input.Length > maxLength)
            throw new ArgumentException($"{input} exceeds maximum length of {maxLength} characters.");
    }

}