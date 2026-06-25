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
 
    private static string RedactSnippet(string? content, int maxLength)
    {
        if (string.IsNullOrEmpty(content))
            return "[no content]";

        var snippet = content.Length > maxLength ? content.Substring(0, maxLength) : content;
        try
        {
            var pattern = "(?i)(api[_-]?key|authorization|bearer)\\s*[:=]\\s*[^\\s,\"']+";
            snippet = System.Text.RegularExpressions.Regex.Replace(snippet, pattern, "[REDACTED]");
        }
        catch
        {      }

        return snippet;
    }

    public async Task<GeminiResponse> GetAiContent(GeminiRequest request, CancellationToken ct = default)
    {
      var payload = new { contents = new[] { new { parts = new[] { new { text = request.Prompt } } } } };
      var response = await _http.PostAsJsonAsync($"v1beta/models/gemini-3-flash-preview:generateContent?key={_apiKey}", payload, ct);

        if (!response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync(ct);
          var snippet = RedactSnippet(content, 500);
          var headers = new Dictionary<string, string?>();
          foreach (var h in response.Headers)
            headers[h.Key] = string.Join(",", h.Value);
          throw new ExternalApiException((int)response.StatusCode, snippet, headers);
        }

      var root = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
      if (root.ValueKind == JsonValueKind.Undefined || root.ValueKind == JsonValueKind.Null)
        throw new HttpRequestException("Empty JSON response from Gemini.");

      if (!root.TryGetProperty("candidates", out var candidates) || candidates.ValueKind != JsonValueKind.Array || candidates.GetArrayLength() == 0)
        throw new HttpRequestException("Gemini response missing 'candidates'.");

      var first = candidates[0];
      if (!first.TryGetProperty("content", out var contentProp) ||
        !contentProp.TryGetProperty("parts", out var parts) ||
        parts.ValueKind != JsonValueKind.Array ||
        parts.GetArrayLength() == 0)
      {
        throw new HttpRequestException("Gemini response has unexpected shape (missing content.parts).");
      }

      var text = parts[0].TryGetProperty("text", out var textProp) ? textProp.GetString() : null;
      return new GeminiResponse { Result = text ?? "Nothing returned" };
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