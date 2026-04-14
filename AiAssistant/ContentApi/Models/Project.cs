namespace ContentApi.Models;


public class Project
{
    public Guid Id { get; set; }

    public string Title {get;set;} = null!;

    public string? Description {get; set;}

    public DateTime Deadline {get; set;}

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public List<AiGeneration> Stories {get; set;} = new();


}