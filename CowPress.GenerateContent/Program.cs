using CowPress.Data;
using CowPress.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
  .UseSqlite("DataSource=../CowPress/app.db;Cache=Shared" ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))
  .Options;

switch (args.FirstOrDefault())
{
  case "import":
    await ImportData(options);
    break;
  case "wipe":
    await WipeData(options);
    break;
  default:
    Console.WriteLine("Usage: dotnet run import|wipe");
    break;
}

async Task ImportData(DbContextOptions<ApplicationDbContext> options)
{
  var random = new Random();
  var files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "output"));

  int imported = 0;

  foreach (var file in files)
  {
    try
    {
      using (var db = new ApplicationDbContext(options))
      {
        if (file.EndsWith("embedding.json")) continue;

        var blogPost = ReadJson<BlogPost>(file);
        if (blogPost is null)
        {
          Console.Error.WriteLine("Failed to deserialize " + file);
          continue;
        }

        blogPost.CreatedAt = GenerateRandomDate(random);

        var embedding = ReadJson<float[]>(file.Replace(".json", ".embedding.json"));
        if (embedding is null)
        {
          Console.Error.WriteLine("Failed to deserialize embedding " + file);
          continue;
        }

        db.Add(blogPost);
        await db.SaveChangesAsync();

        db.Add(new Embedding() { Id = blogPost.Id, Vector = embedding });
        await db.SaveChangesAsync();

        Console.WriteLine("Processed " + file);
        ++imported;
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error on file: {file}, {ex.Message}");
    }
  }
  
  Console.WriteLine($"Imported {imported} files");
}

async Task WipeData(DbContextOptions<ApplicationDbContext> options)
{
  using (var db = new ApplicationDbContext(options))
  {
    db.BlogPosts.RemoveRange(db.BlogPosts);
    db.Embeddings.RemoveRange(db.Embeddings);
    await db.SaveChangesAsync();
  }
}

T? ReadJson<T>(string filePath) where T : class
{
  var json = File.ReadAllText(filePath);
  return JsonSerializer.Deserialize<T>(json);
}

DateTime GenerateRandomDate(Random random)
{
  var start = new DateTime(DateTime.Now.Year, 1, 1);
  var range = (DateTime.Today - start).Days;
  return start.AddDays(random.Next(range)).AddHours(random.Next(24)).AddMinutes(random.Next(60)).AddSeconds(random.Next(60));
}