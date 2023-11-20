namespace CowPress.Utils;
public static class StaticFileExtensions
{
  public static void UseStaticFilesWithPreCompressedSupport(this IApplicationBuilder app)
  {
    app.UseStaticFiles(new StaticFileOptions
    {
      OnPrepareResponse = context =>
      {
        var headers = context.Context.Response.Headers;

        if (headers.ContentType == "application/x-gzip")
        {
          var contentTypeMappings = new Dictionary<string, string>
          {
            { ".js.gz", "application/javascript" },
            { ".css.gz", "text/css" }
          };
          var fileName = Path.GetFileName(context.File.Name);
          var fileExtension = Path.GetExtension(Path.GetFileNameWithoutExtension(fileName)) + Path.GetExtension(fileName);

          if (contentTypeMappings.TryGetValue(fileExtension, out var contentType))
          {
            headers.ContentType = contentType;
          }

          headers.Append("Content-Encoding", "gzip");
        }
      }
    });
  }
}