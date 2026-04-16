using AiAssistant.ContentApi.Data;
using ContentApi.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public sealed class GeneratedTopicsHandler(
    AppDbContext db,
    ILlmProxyClient llmProxyClient)
    : IRequestHandler<GenerateTopicsFromLlmCommand, IReadOnlyList<NotebookTopicsSummary>>
{
    private readonly AppDbContext _db = db;
    private readonly ILlmProxyClient _llmProxyClient = llmProxyClient;

    public async Task<IReadOnlyList<NotebookTopicsSummary>> Handle(GenerateTopicsFromLlmCommand request, CancellationToken ct)
    {
        var notebookExists = await _db.Notebooks
            .AnyAsync(n => n.Id == request.NotebookId, ct);
        if(!notebookExists) throw new NotebookNotFoundException("Notebook was not found!");
       
        var generatedContent = await _llmProxyClient.GenerateTopicsAsync(ct);
        if (generatedContent.Count == 0) return [];


        var currentMaxOrder = await _db.Topics
            .Where(t => t.NotebookId == request.NotebookId)
            .Select(t => (int?)t.Order)
            .MaxAsync(ct) ?? 0;

        var topics = new List<Topic>();
        var nextOrder = currentMaxOrder;
        foreach(var title in generatedContent)
        {
            var t = title.Trim(); 
            if (string.IsNullOrWhiteSpace(t)) continue;
            
            nextOrder++;

            topics.Add(new Topic
            {
                Id = Guid.NewGuid(),
                NotebookId = request.NotebookId,
                Title = title,
                Order = nextOrder,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            });
        }

        if(topics.Count == 0) return [];

        _db.Topics.AddRange(topics);
            await _db.SaveChangesAsync(ct);
            
        return [.. topics
            .Select
            (t => new NotebookTopicsSummary
                (
                t.Title,
                t.Order,
                t.IsCompleted
                )
            )];

    }
}