using CowPress.Data;
using CowPress.Models;
using Microsoft.EntityFrameworkCore;

public class RelatedBlogPosts
{
    private readonly ApplicationDbContext _context;

    public RelatedBlogPosts(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<BlogPost>> FetchRelatedPostsAsync(BlogPost blogPost)
    {
        var postEmbedding = await _context.Embeddings.FirstOrDefaultAsync(x => x.Id == blogPost.Id);
        var embeddings = await _context.Embeddings.Where(x => x.Id != blogPost.Id).ToListAsync();
        var relatedPostsEmbeddings = embeddings
            .OrderByDescending(x => CosineSimilarity(postEmbedding.Vector, x.Vector))
            .Take(3)
            .Select(x => x.Id)
            .ToList();
        
        var relatedPosts = await _context.BlogPosts
            .Where(x => relatedPostsEmbeddings.Contains(x.Id))
            .ToListAsync();
        return relatedPosts;
    }

    private float CosineSimilarity(IReadOnlyList<float> v1, IReadOnlyList<float> v2)
    {
        var N = Math.Min(v1.Count, v2.Count);
        var dot = 0.0;
        var mag1 = 0.0;
        var mag2 = 0.0;
        for (var n = 0; n < N; n++)
        {
            dot += v1[n] * v2[n];
            mag1 += Math.Pow(v1[n], 2);
            mag2 += Math.Pow(v2[n], 2);
        }

        return (float) (dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2)));
    }
}