using System.ComponentModel.DataAnnotations;

namespace CowPress.Models;

public class BlogPost
{
    public int Id { get; set; }

    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Content { get; set; }

    [Required]
    public string? Author { get; set; }

    public DateTime CreatedAt { get; set; }
}
