using Azure;
using Azure.AI.OpenAI;

namespace CowPress.Pages.Post;

public class ContentGenerator
{
    private const string ContentGenerationDeploymentName = "gpt-4";
    private const string EmbeddingDeploymentName = "text-embedding-ada-002";

    private readonly OpenAIClient _openAiClient;

    public ContentGenerator(OpenAIClient openAiClient)
    {
        _openAiClient = openAiClient;
    }

    public async Task<string> GenerateContent(string subject)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = ContentGenerationDeploymentName,
            Messages =
            {
                new ChatMessage(ChatRole.System, $"Create a blog post in 250 characters at most"),
                new ChatMessage(ChatRole.User, subject),
            },
            MaxTokens = 250
        };

        Response<ChatCompletions> response = await _openAiClient.GetChatCompletionsAsync(chatCompletionsOptions);

        var firstResponseMessage = response.Value.Choices.First().Message.Content;
        return firstResponseMessage;
    }

    public IEnumerable<float> GenerateEmbedding(string text)
    {
        var embeddingOptions = new EmbeddingsOptions(EmbeddingDeploymentName, new List<string> {text});
        var returnValue = _openAiClient.GetEmbeddings(embeddingOptions);
        var embedding = returnValue.Value.Data.First().Embedding;

        var embeddingList = embedding.Span.ToArray().ToList();
        return embeddingList;
    }
}