using AiAssistant.ContentApi.Data;
using AiAssistant.ContentApi.DTO;
using ContentApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ContentApi.Services;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AiContentDb"));

builder.Services.AddOpenApi();

builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<AiGenerationRepository>();

builder.Services.AddScoped<IProjectService<ProjectRequest, ProjectResponse, Project>, ProjectService>();
builder.Services.AddScoped<IAiGenerationService<AiGenerationRequest, AiGenerationResponse, AiGeneration>, AiGenerationService>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

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