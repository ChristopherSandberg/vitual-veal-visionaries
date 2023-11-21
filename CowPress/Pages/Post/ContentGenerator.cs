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
    
    public async Task<string> GenerateContent(string subject) {

        OpenAIClient client = new(new Uri("https://dev-week-2023.openai.azure.com/"), new AzureKeyCredential("7b68f68e0d7943fbb990eb83e4c02e1d"));

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "gpt-4", //This must match the custom deployment name you chose for your model
            Messages =
            {
                new ChatMessage(ChatRole.System, $"Create a blog post in 250 characters at most"),
                new ChatMessage(ChatRole.User, subject),
            },
            MaxTokens = 250
        };

        Response<ChatCompletions> response = client.GetChatCompletions(chatCompletionsOptions);

        Console.WriteLine(response.Value.Choices[0].Message.Content);

        return response.Value.Choices[0].Message.Content;


        /*
        var request = new CompletionsOptions()
        {
            DeploymentName = "gpt-4", // assumes a matching model deployment or model name
            Prompts = { "Hello, world!" },
        };
        var response = await _openAIClient.GetCompletionsAsync(request);

        return response.Value.Choices.FirstOrDefault().Text;
        */
    }

    public async Task<IReadOnlyList<float>> GenerateEmbedding(string text) {
        // TODO Generate embedding from the text

        return await Task.FromResult(new List<float>());
    }
}