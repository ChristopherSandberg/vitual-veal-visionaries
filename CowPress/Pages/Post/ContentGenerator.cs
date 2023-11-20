namespace CowPress.Pages.Post;

public class ContentGenerator
{
    public async Task<string> GenerateContent(string subject) {
        // TODO Implement AI

        return await Task.FromResult("Lorem Ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue");
    }

    public async Task<IReadOnlyList<float>> GenerateEmbedding(string text) {
        // TODO Generate embedding from the text

        return await Task.FromResult(new List<float>());
    }
}