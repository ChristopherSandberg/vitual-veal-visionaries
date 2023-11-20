using CowPress.Data;
using CowPress.Models;
using Microsoft.EntityFrameworkCore;
namespace CowPress.Pages.Post;

public class BlogSearch
{
  private readonly ApplicationDbContext _context;

  public BlogSearch(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<List<BlogPost>> SearchPosts(string search)
  {
    var blogPosts = await _context.BlogPosts
      .Where(x => x.Title != null && x.Title.ToLower().Contains(search.ToLower()))
      .OrderByDescending(x => x.CreatedAt)
      .ToListAsync();

    return blogPosts;
  }
}