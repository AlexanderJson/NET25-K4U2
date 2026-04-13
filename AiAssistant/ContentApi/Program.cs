using AiAssistant.ContentApi.Data;
using AiAssistant.ContentApi.DTO;
using AiAssistant.ContentApi.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AiContentDb"));

builder.Services.AddOpenApi();

// Repository
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<AiGenerationRepository>();

// Services
builder.Services.AddScoped<IProjectService<ProjectRequest, ProjectResponse, Project>, ProjectService>();
builder.Services.AddScoped<IAiGenerationService<AiGenerationRequest, AiGenerationResponse, AiGeneration>, AiGenerationService>();

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