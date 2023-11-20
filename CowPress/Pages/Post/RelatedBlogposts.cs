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
    // Fetch blog posts that have the same category as the given blog post
    var relatedPosts = (await _context.BlogPosts.ToListAsync())
                .OrderBy(x => Guid.NewGuid())
                .Take(3).ToList();

    return relatedPosts;
  }
}