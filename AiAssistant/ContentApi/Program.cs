using AiAssistant.ContentApi.Data;
using ContentApi.Features.User.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;



var builder = WebApplication.CreateBuilder(args);
 

// Controllers
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AiContentDb"));

builder.Services.AddOpenApi();
 builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly));
builder.Services.AddScoped<IUserQueries, UserQueries>();
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