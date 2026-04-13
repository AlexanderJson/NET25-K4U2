using AiAssistant.ContentApi.Data;
using AiAssistant.ContentApi.DTO;
using AiAssistant.ContentApi.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AiContentDb"));


builder.Services.AddOpenApi();

var app = builder.Build();
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<AiGenerationRepository>();

builder.Services.AddScoped<IProjectService<ProjectRequest, ProjectResponse, Project>, ProjectService>();
builder.Services.AddScoped<IAiGenerationService<AiGenerationRequest, AiGenerationResponse, AiGeneration>, AiGenerationService>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();