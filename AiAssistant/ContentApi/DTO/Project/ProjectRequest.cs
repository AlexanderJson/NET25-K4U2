using ContentApi.Models;

public class ProjectRequest
{

    public string Title {get;set;} = null!;

    public string? Description {get; set;}

    public DateTime Deadline {get; set;}

}
