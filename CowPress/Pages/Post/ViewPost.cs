using CowPress.Data;
using CowPress.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CowPress.Pages.Post;

public class ViewPostModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly RelatedBlogPosts _relatedBlogPosts;

    public ViewPostModel(ApplicationDbContext context, RelatedBlogPosts relatedBlogPosts)
    {
        _context = context;
        _relatedBlogPosts = relatedBlogPosts;
    }

    public BlogPost BlogPost { get; set; } = default!;
    public IList<BlogPost> RelatedBlogPosts { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        BlogPost = post;

        RelatedBlogPosts = await _relatedBlogPosts.FetchRelatedPostsAsync(BlogPost);

        return Page();
    }
}
