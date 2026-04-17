using AiAssistant.ContentApi.Data;
using ContentApi.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

public sealed record GenerateTopicsCommand(Guid notebookId) : IRequest<GenerateTopicsResponse>;
public sealed record GenerateTopicsResponse(string Result);

public sealed class GeneratedTopicsHandler
    (
        AppDbContext db,
        ILlmClient llmProxyClient,
        ITopicPromptBuilder builder
    ) : IRequestHandler<GenerateTopicsCommand, GenerateTopicsResponse>
    
    {

        ILlmClient _client = llmProxyClient;
        ITopicPromptBuilder _builder = builder;
    public async Task<GenerateTopicsResponse> Handle(GenerateTopicsCommand request, CancellationToken ct)
    {
        var id = request.notebookId;
        Guard.Against.NullOrEmptyGuid(id);
        // the proxy only need title+category to generate topics, so we only fetch those
        var notebook = await db.Notebooks
            .AsNoTracking() // + this works since we dont change the notebook 
            .Where(n=> n.Id == request.notebookId)
            .Select(n=> new {n.Title, n.Category})
            .FirstOrDefaultAsync(ct);
        Guard.Against.Null(notebook);
        // bit overkill maybe but rather safe than sorry!
        Guard.Against.NullOrWhiteSpace(notebook.Category);
        Guard.Against.NullOrWhiteSpace(notebook.Title);
        
        // this returns a string of instructions (aka full prompt)
        var prompt = _builder.Generate(notebook.Title, notebook.Category);

        // now client gets the prompt
        var generatedContent = await _client.GenerateTopicsAsync(prompt, ct);

        Console.WriteLine("=== FULL RAW JSON FROM PROXY ===");
        Console.WriteLine(generatedContent);
        Console.WriteLine("=== END RAW JSON ===");
        var resp = new GenerateTopicsResponse(generatedContent);

        return resp;
    }
 
    private static bool _promptToTopics(string topics)
    {
        var regex = @"TI:\s*(.+?)(?=(?:\\n|""\s*,|\r?\n|$))";
        
        return true;
    } 
}




/*

    private readonly AppDbContext _db = db;
    private readonly ITopicPromptBuilder _promptBuilder = promptBuilder;
    private readonly ILlmClient _llmProxyClient = llmProxyClient;

    public async Task<IReadOnlyList<string>> Handle(
        GenerateTopicsFromLlmCommand request,
        CancellationToken ct)
    {
        var notebook = await _db.Notebooks
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == request.NotebookId, ct)
            ?? throw new NotebookNotFoundException("Notebook was not found!");

        var prompt = _promptBuilder.Generate(notebook.Title, notebook.Category);

        var generatedContent = await _llmProxyClient.GenerateTopicsAsync(prompt, ct);

        Console.WriteLine("=== GENERATED CONTENT IN HANDLER ===");
        foreach (var topic in generatedContent)
        {
            Console.WriteLine(topic);
        }

        return generatedContent;
    }
*/