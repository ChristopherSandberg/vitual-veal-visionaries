using CowPress.Data;
using CowPress.Models;
using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace CowPress.Pages.Post;

[Authorize]
public class EditPostModel : PageModel
{
  private readonly ApplicationDbContext _context;

  public EditPostModel(ApplicationDbContext context)
  {
    _context = context;
  }

  [BindProperty]
  public BlogPost BlogPost { get; set; } = default!;

  public async Task<IActionResult> OnGetAsync(int id)
  {
    var post = await _context.BlogPosts.FindAsync(id);

    if (post == null)
    {
      return NotFound();
    }

    BlogPost = post;

    return Page();
  }

  public IActionResult OnGetModal()
  {
      return Partial("_Modal");
  }

  public async Task<IActionResult> OnPostDeleteAsync()
  {
    var blogPost = await _context.BlogPosts.FindAsync(BlogPost.Id);

    if (blogPost == null)
    {
      return NotFound();
    }

    _context.BlogPosts.Remove(blogPost);
    await _context.SaveChangesAsync();

    Response.Htmx(h => {
        h.Redirect("/post")
        .WithTrigger("deleted");
    });

    return new EmptyResult();
  }

  public async Task<IActionResult> OnPostUpdatePostAsync()
  {
      if (BlogPost == null)
      {
          return NotFound();
      }

      var existingBlogPost = await _context.BlogPosts.AsNoTracking().FirstOrDefaultAsync(b => b.Id == BlogPost.Id);

      if (existingBlogPost == null)
      {
          return NotFound();
      }

      _context.Attach(BlogPost);

      _context.Entry(BlogPost).Property(b => b.Author).IsModified = true;
      _context.Entry(BlogPost).Property(b => b.Title).IsModified = true;
      _context.Entry(BlogPost).Property(b => b.Content).IsModified = true;

      await _context.SaveChangesAsync();

      return RedirectToPage("/Post/BlogPosts");
  }
}
