using Azure;
using Azure.AI.OpenAI;

namespace CowPress.Pages.Post;

public class ContentGenerator
{
    public const string DeploymentName = "gpt-4";
    public readonly OpenAIClient _openAIClient;

    public ContentGenerator(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

    public async Task<string> GenerateContent(string subject)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = DeploymentName,
            Messages =
            {
                new ChatMessage(ChatRole.System, $"Create a blog post in 250 characters at most"),
                new ChatMessage(ChatRole.User, subject),
            },
            MaxTokens = 250
        };

        Response<ChatCompletions> response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);

        var firstResponseMessage = response.Value.Choices.First().Message.Content;
        return firstResponseMessage;
    }

    public IReadOnlyList<float> GenerateEmbedding(string text)
    {
        EmbeddingsOptions embeddingOptions = new EmbeddingsOptions("text-embedding-ada-002", new List<string> { text });

        var returnValue = _openAIClient.GetEmbeddings(embeddingOptions);

        var embedding = returnValue.Value.Data.First().Embedding;
        
        var embeddingList =  embedding.Span.ToArray().ToList();
        
        return embeddingList;
    }
}