using AiAssistant.ContentApi.Data;
using ContentApi.DTO;
using ContentApi.Projection;
using ContentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AiContentDb"));

builder.Services.Configure<LlmPromptOptions>(
    builder.Configuration.GetSection("LlmPrompts"));

builder.Services.AddScoped<ITopicPromptBuilder, TopicPromptBuilder>();

builder.Services.AddHttpClient<ILlmClient, LlmClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5002/");
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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();