
namespace AiAssistant.ContentApi.DTO;
public class AiGenerationResponse
{
       public Guid Id { get; set; }

        public string Prompt { get; set; } = null!;

        public string Response { get; set; } = null!;


        public DateTime CreatedAt { get; set; }

        public Guid? ProjectId { get; set; } 
}