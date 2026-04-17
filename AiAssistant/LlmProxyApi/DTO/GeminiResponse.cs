using System.ComponentModel.DataAnnotations;

public record GeminiResponse
{
    [Required]
    [MinLength(1)]
    public string Result {get;set;} = string.Empty;
    
}