using AiAssistant.ContentApi.Data;
using ContentApi.Api.Middleware;
using ContentApi.DTO;
using ContentApi.Projection;
using ContentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AiContentDb"));

builder.Services.Configure<LlmPromptOptions>(
    builder.Configuration.GetSection("LlmPrompts"));

builder.Services.AddScoped<ITopicPromptBuilder, TopicPromptBuilder>();

builder.Services.AddOptions<LlmProxyOptions>()
    .Bind(builder.Configuration.GetSection(LlmProxyOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.BaseUrl),
        "LlmProxy BaseUrl is required.")
    .Validate(options => options.TimeoutSeconds > 0,
        "LlmProxy TimeoutSeconds must be greater than 0.")
    .ValidateOnStart();

builder.Services.AddHttpClient<ILlmClient, LlmClient>((serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<LlmProxyOptions>>()
        .Value;

    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
});

builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddScoped<INotebookRepository, NotebookRepository>();
builder.Services.AddScoped<INotebookQueries, NotebookQueries>();
builder.Services.AddScoped<INotebookService, NotebookService>();

builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<ITopicQueries, TopicQueries>();
builder.Services.AddScoped<ITopicService, TopicService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserQueries, UserQueries>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<NotebookWorkflowService>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();