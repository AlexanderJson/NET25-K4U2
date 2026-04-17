using Google.GenAI;

public class GeminiService
{
    public static async Task main() {
    var client = new Client();
    var response = await client.Models.GenerateContentAsync(
      model: "gemini-3-flash-preview", contents: "Explain how AI works in a few words"
    );
    Console.WriteLine(response.Candidates[0].Content.Parts[0].Text);
  }
}

