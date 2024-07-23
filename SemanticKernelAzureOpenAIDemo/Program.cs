using Microsoft.KernelMemory;
using Microsoft.KernelMemory.ContentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;


var memory = new KernelMemoryBuilder()
    //.WithAzureOpenAITextEmbeddingGeneration()
    .WithAzureOpenAITextEmbeddingGeneration(
        new AzureOpenAIConfig
        {
            APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
            Endpoint = "YOUR ENDPOINT",
            Auth = AzureOpenAIConfig.AuthTypes.APIKey,
            APIKey = "YOU API KEY",
            Deployment = "YOUR DEPLOYMENT NAME"
        }
    )
    .WithAzureOpenAITextGeneration(
    new AzureOpenAIConfig
    {
        APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
        Endpoint = "YOUR ENDPOINT",
        Auth = AzureOpenAIConfig.AuthTypes.APIKey,
        APIKey = "YOUR API KEY",
        Deployment = "YOUR DEPLOYMEN NAME",
        MaxRetries = 10
    }).WithSimpleFileStorage(new SimpleFileStorageConfig()
    {
        Directory = "YOUR PATH",
        StorageType = FileSystemTypes.Disk
    })
    .WithSimpleVectorDb(new Microsoft.KernelMemory.MemoryStorage.DevTools.SimpleVectorDbConfig()
    {
        Directory = "YOUR PATH",
        StorageType = FileSystemTypes.Disk
    })
    .Build<MemoryServerless>();


await memory.ImportDocumentAsync("YOUR PDF.pdf", documentId: "doc001");

Console.WriteLine("Ask a question about this document: ");
var question = Console.ReadLine();

var answer = await memory.AskAsync(question!);

Console.WriteLine($"Question: {question}\n\nAnswer: {answer.Result}");

Console.WriteLine("Sources:\n");

foreach (var source in answer.RelevantSources)
{
    Console.WriteLine($"{source.SourceName} - {source.Link} [{source.Partitions.First().LastUpdate:D}]");
}