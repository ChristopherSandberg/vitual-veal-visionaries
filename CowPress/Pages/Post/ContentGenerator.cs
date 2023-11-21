using Azure;
using Azure.AI.OpenAI;
using static System.Environment;

namespace CowPress.Pages.Post;

public class ContentGenerator
{
    public readonly OpenAIClient _openAIClient;

    public ContentGenerator(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

    public async Task<string> GenerateContent(string subject)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "gpt-4",
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

    public async Task<IReadOnlyList<float>> GenerateEmbedding(string text)
    {
        // TODO Generate embedding from the text

        return await Task.FromResult(new List<float>());
    }
}