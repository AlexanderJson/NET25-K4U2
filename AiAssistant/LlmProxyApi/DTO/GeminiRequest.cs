using System.ComponentModel.DataAnnotations;

public record GeminiRequest
{

    public string Prompt {get;set;} = string.Empty;
    
}