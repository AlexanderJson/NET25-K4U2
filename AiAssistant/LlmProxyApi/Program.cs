using LlmProxyApi.Service;
using Scalar.AspNetCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var geminiBase = builder.Configuration["Gemini:BaseUrl"] ?? "https://generativelanguage.googleapis.com/";
var geminiTimeout = builder.Configuration.GetValue<int?>("Gemini:TimeoutSeconds") ?? 15;
builder.Services.AddHttpClient<GeminiService>(client =>
{
    client.BaseAddress = new Uri(geminiBase);
    client.Timeout = TimeSpan.FromSeconds(geminiTimeout);

    var apiKey = builder.Configuration["Gemini:ApiKey"];

    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);
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