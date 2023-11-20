using CowPress.Data;
using CowPress.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace CowPress.Pages.Post;

public class BlogPostsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly BlogSearch _blogSearch;

    public BlogPostsModel(ApplicationDbContext context, BlogSearch blogSearch)
    {
        _context = context;
        _blogSearch = blogSearch;
        BlogPosts = new List<BlogPost>();
        Search = "";
    }

    public IList<BlogPost> BlogPosts { get; set; }
    
    [FromQuery(Name = "search")]
    public string Search { get; set; }

    public async Task OnGetAsync()
    {
        if(Search == "" || Search == null)
            BlogPosts = _context.BlogPosts.OrderByDescending(x => x.CreatedAt).ToList();
        else
            BlogPosts = await _blogSearch.SearchPosts(Search);
    }

    public async Task<IActionResult> OnGetSearchAsync()
    {
        BlogPosts = await _blogSearch.SearchPosts(Search);

        return Partial("_BlogPostsPartial", this);
    }
}
