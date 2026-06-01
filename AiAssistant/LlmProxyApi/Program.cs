using LlmProxyApi.Middleware;
using LlmProxyApi.Service;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOptions<GeminiOptions>()
    .Bind(builder.Configuration.GetSection(GeminiOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.BaseUrl),
        "Gemini BaseUrl is required.")
    .Validate(options => !string.IsNullOrWhiteSpace(options.GenerateContentEndpoint),
        "Gemini GenerateContentEndpoint is required.")
    .Validate(options => !string.IsNullOrWhiteSpace(options.ApiKey),
        "Gemini ApiKey is required.")
    .ValidateOnStart();

builder.Services.AddHttpClient<GeminiService>((serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<GeminiOptions>>()
        .Value;

    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);

    client.DefaultRequestHeaders.Add(
        options.ApiKeyHeaderName,
        options.ApiKey);
});
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.Run();