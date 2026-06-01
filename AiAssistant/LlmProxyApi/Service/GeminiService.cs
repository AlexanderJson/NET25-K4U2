using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
namespace LlmProxyApi.Service;
public class GeminiService
{
    private readonly HttpClient _http;
    //private readonly string _apiKey;
    private readonly GeminiOptions _options;


  public GeminiService(HttpClient http, IOptions<GeminiOptions> options)
  {
      _http = http;
      _options = options.Value;
      //_apiKey = config["Gemini:ApiKey"] ?? throw new InvalidOperationException("Gemini API key is missing.");
  }
 
  public async Task<GeminiResponse> GetAiContent(GeminiRequest request, CancellationToken ct)
  {
      var payload = new { contents = new[] { new { parts = new[] { new { text = request.Prompt } } } } };
      
      try
      {
          using var response = await _http.PostAsJsonAsync(
                _options.GenerateContentEndpoint,
                payload,
                ct);
          if(!response.IsSuccessStatusCode) throw await CreateGeminiExceptionAsync(response, ct);

          var root = await response.Content.ReadFromJsonAsync<JsonElement>(ct);
          if (!root.TryGetProperty("candidates", out var candidates) ||  candidates.GetArrayLength() == 0)
            {
                throw new GeminiApiException(
                    "Gemini returned no candidates.",
                    GeminiStatus.Unknown);
            }
            var resultText = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

                  if (string.IsNullOrWhiteSpace(resultText))
            {
                throw new GeminiApiException(
                    "Gemini returned an empty response.",
                    GeminiStatus.Unknown);
            }

            return new GeminiResponse
            {
                Result = resultText
            };
        }
        catch (TaskCanceledException exception) when (!ct.IsCancellationRequested)
        {
            throw new GeminiApiException(
                "Gemini request timed out.",
                GeminiStatus.DeadlineExceeded,
                innerException: exception);
        }
        catch (HttpRequestException exception)
        {
            throw new GeminiApiException(
                "Could not reach Gemini API.",
                GeminiStatus.Unavailable,
                innerException: exception);
        }
        catch (JsonException exception)
        {
            throw new GeminiApiException(
                "Gemini returned invalid JSON.",
                GeminiStatus.Unknown,
                innerException: exception);
        }
  }
      private static async Task<GeminiApiException> CreateGeminiExceptionAsync(

        HttpResponseMessage response,

        CancellationToken cancellationToken)

    {

        GeminiErrorResponse? geminiError = null;



        try

        {

            geminiError = await response.Content.ReadFromJsonAsync<GeminiErrorResponse>(

                cancellationToken);

        }

        catch

        {

        }



        var geminiStatus = GeminiStatusMapper.Map(geminiError?.Error.Status);



        return new GeminiApiException(

            message: "Gemini API request failed.",

            geminiStatus: geminiStatus,

            geminiCode: geminiError?.Error.Code ?? (int)response.StatusCode);

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



  public sealed record GeminiErrorResponse(GeminiError Error);

public sealed record GeminiError(
    int Code,
    string Message,
    string Status);

public enum GeminiStatus
{
    InvalidArgument,
    FailedPrecondition,
    PermissionDenied,
    NotFound,
    ResourceExhausted,
    Internal,
    Unavailable,
    DeadlineExceeded,
    Unknown
}

public static class GeminiStatusMapper
{
    public static GeminiStatus Map(string? status)
    {
        return status switch
        {
            "INVALID_ARGUMENT" => GeminiStatus.InvalidArgument,
            "FAILED_PRECONDITION" => GeminiStatus.FailedPrecondition,
            "PERMISSION_DENIED" => GeminiStatus.PermissionDenied,
            "NOT_FOUND" => GeminiStatus.NotFound,
            "RESOURCE_EXHAUSTED" => GeminiStatus.ResourceExhausted,
            "INTERNAL" => GeminiStatus.Internal,
            "UNAVAILABLE" => GeminiStatus.Unavailable,
            "DEADLINE_EXCEEDED" => GeminiStatus.DeadlineExceeded,
            _ => GeminiStatus.Unknown
        };
    }
}

public sealed class GeminiApiException : Exception
{
    public GeminiStatus GeminiStatus { get; }
    public int? GeminiCode { get; }

    public GeminiApiException(
        string message,
        GeminiStatus geminiStatus = GeminiStatus.Unknown,
        int? geminiCode = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        GeminiStatus = geminiStatus;
        GeminiCode = geminiCode;
    }
}