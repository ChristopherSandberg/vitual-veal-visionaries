using CowPress.Data;
using CowPress.Models;
using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CowPress.Pages.Post;

[Authorize]
public class CreatePostModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ContentGenerator _contentGenerator;

    public CreatePostModel(ApplicationDbContext context, ContentGenerator contentGenerator)
    {
      _context = context;
      _contentGenerator = contentGenerator;
    }

    [BindProperty]
    public BlogPost? BlogPost { get; set; }

    public void OnGet()
    {

    }

    public IActionResult OnGetModal()
    {
        return Partial("_Modal");
    }

    public async Task<IActionResult> OnPostGenerateContentAsync(string subject)
    {
        return Content(await _contentGenerator.GenerateContent(subject));
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid && BlogPost is not null)
        {
            var post = new BlogPost
            {
                Title = BlogPost.Title,
                Content = BlogPost.Content,
                Author = BlogPost.Author,
                CreatedAt = DateTime.Now
            };

            _context.BlogPosts.Add(post);
            _context.SaveChanges();


            Response.Htmx(h => {
                h.Redirect("/post")
                .WithTrigger("deleted");
            });

            return new EmptyResult();
        }

        return Page();
    }
}
