using LlmProxyApi.Service;
using Scalar.AspNetCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// CHECK: Gemini HttpClient configuration is driven by configuration (no hardcoded keys)
// - Reads `Gemini:BaseUrl` and `Gemini:TimeoutSeconds` from configuration/environment.
// - Applies Authorization header only when `Gemini:ApiKey` is present in configuration (not hardcoded).
var geminiBase = builder.Configuration["Gemini:BaseUrl"] ?? "https://generativelanguage.googleapis.com/";
var geminiTimeout = builder.Configuration.GetValue<int?>("Gemini:TimeoutSeconds") ?? 15;
builder.Services.AddHttpClient<GeminiService>(client =>
{
    client.BaseAddress = new Uri(geminiBase);
    client.Timeout = TimeSpan.FromSeconds(geminiTimeout);
    var apiKey = builder.Configuration["Gemini:ApiKey"];
    if (!string.IsNullOrEmpty(apiKey))
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
    }
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.Run();