using CowPress.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CowPress.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Embedding> Embeddings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
