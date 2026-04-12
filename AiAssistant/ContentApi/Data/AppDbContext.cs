using Microsoft.EntityFrameworkCore;
using AiAssistant.ContentApi.Models;

namespace AiAssistant.ContentApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AiGeneration> AiGenerations => Set<AiGeneration>();

    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<AiGeneration>()
            .HasOne(g => g.Project)
            .WithMany(p => p.Stories)
            .HasForeignKey(g => g.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}