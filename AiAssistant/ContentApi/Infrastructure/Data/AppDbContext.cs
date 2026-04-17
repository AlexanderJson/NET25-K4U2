using Microsoft.EntityFrameworkCore;
using ContentApi.Models;

namespace AiAssistant.ContentApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Notebook> Notebooks => Set<Notebook>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.Property(u => u.HashedPassword)
                .IsRequired();

            entity.Property(u => u.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<Notebook>(entity =>
        {
            entity.HasKey(n => n.Id);

            entity.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(n => n.Category)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(n => n.CreatedAt)
                .IsRequired();

            entity.Property(n => n.LastUpdated)
                .IsRequired();

            entity.Property(n => n.RowVersion)
                .IsRowVersion();

            entity.HasOne(n => n.User)
                .WithMany(u => u.Notebooks)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(n => n.UserId);
            entity.HasIndex(n => n.Category);
            entity.HasIndex(n => new { n.UserId, n.Category });
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Order)
                .IsRequired();

            entity.Property(t => t.IsCompleted)
                .IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            entity.Property(t => t.RowVersion)
                .IsRowVersion();

            entity.HasOne(t => t.Notebook)
                .WithMany(n => n.Topics)
                .HasForeignKey(t => t.NotebookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(t => new { t.NotebookId, t.Order })
                .IsUnique();

            entity.HasIndex(t => t.NotebookId);
            entity.HasIndex(t => t.IsCompleted);
            entity.HasIndex(t => new { t.NotebookId, t.IsCompleted });
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(n => n.Id);

            entity.Property(n => n.EncryptedContent)
                .IsRequired();

            entity.Property(n => n.CreatedAt)
                .IsRequired();

            entity.Property(n => n.LastUpdated)
                .IsRequired();

            entity.Property(n => n.RowVersion)
                .IsRowVersion();

            entity.HasOne(n => n.Topic)
                .WithMany(t => t.Notes)
                .HasForeignKey(n => n.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(n => n.TopicId);
        });
    }
}