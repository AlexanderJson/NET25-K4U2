using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
namespace LlmProxyApi.Service;
public class GeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

  public GeminiService(HttpClient http, IConfiguration config)
  {
      _http = http;
      _apiKey = config["Gemini:ApiKey"] ?? "";
  }
 
  public async Task<GeminiResponse> GetAiContent(GeminiRequest request)
  {
      var payload = new { contents = new[] { new { parts = new[] { new { text = request.Prompt } } } } };
      var response = await _http.PostAsJsonAsync($"v1beta/models/gemini-3-flash-preview:generateContent?key={_apiKey}", payload);
      response.EnsureSuccessStatusCode();
      var root = await response.Content.ReadFromJsonAsync<JsonElement>();
      var resultText = root.GetProperty("candidates")[0]
                         .GetProperty("content")
                         .GetProperty("parts")[0]
                         .GetProperty("text")
                         .GetString();

      return new GeminiResponse { Result = resultText ?? "Nothing returned" };
  }
}



    /*
    public static async Task main() {
    var client = new Client();
    var response = await client.Models.GenerateContentAsync(
      model: "gemini-3-flash-preview", contents: "Explain how AI works in a few words"
    );
    Console.WriteLine(response.Candidates[0].Content.Parts[0].Text);
  }*/