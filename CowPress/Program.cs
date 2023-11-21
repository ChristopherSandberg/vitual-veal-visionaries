using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CowPress.Data;
using CowPress.Utils;
using CowPress.Pages.Post;
using Azure.AI.OpenAI;
using Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false; })
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Lax password requirements because customer require it, see COW-1296
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddSingleton(new OpenAIClient(
        new Uri("https://dev-week-2023.openai.azure.com/"),
        new AzureKeyCredential("7b68f68e0d7943fbb990eb83e4c02e1d")));

builder.Services.AddTransient<ContentGenerator>();
builder.Services.AddTransient<RelatedBlogPosts>();
builder.Services.AddTransient<BlogSearch>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFilesWithPreCompressedSupport();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    app.UseRouteDebugger();
}

Console.WriteLine(@"

                                                                                                                             
                                                                                                                             
                                                                                                                             
                                                                                                                             
                                                                                                                             
           .%@@@@                                            ::                                                              
        @@@@@@@@@@@@@-                                    @@@@@@@@@@@@@%                                              :@@@@  
     %@@@@@@@@@@@@@@@@                                    @@@@@*#@@@@@@@%                                              @@@@  
    @@@@@@:       :@          .                                     %@@@%*:.    ==      ::            +=           -+        
   @@@@@=                -+*%%%%%#*= :+#*%     **#:    ****-:        @@@@@@@@=@@@@@:@@@@@@@@@@    @@@@@@@@@@   @@@@@@@@@@@   
   @@@@@                **##+=:-+#***: =**.   :*+*=    *++*         @@@@@@@@@@@@@  @@@@@:=@@@@@  @@@@-  @@@@  -@@@=  %@@#    
   @@@@@              .=**:        **=. .*+:  ++==*. .=+==. :@@@@@@@@@@%#%@@%     @@%        @@@@@@@          #@@@           
   @@@@@              =+++         ==+*  **= -*++=*- =+++= *@@@@@@@@@+  =%@@     @@@@@@@@@@@@@@@@=@@@@@@@%:    @@@@@@@@*     
   @@@@@@             =+**         =+**  #++=++==:*+=++*-: @@%          @@@@     @@@:                :@@@@@@%      %@@@@@@   
    +@@@@@@:     @@@-  :#*=:      -**+:  ==+=++:  ++==+#  @@@%          @@@@     *@@@@                   *@@@          @@@@  
      @@@@@@@@@@@@@@@@  =*#%%#*##%%*#:    =***+   ****+*  @@@@          @@@@:      @@@@@@@@@@@@  @@@@@@@@@@@@%=@@@@@=@@@@@   
        @@@@@@@@@@@@+     -+*#%#**=:      -*+=-   -=**-   @@@@.         @@@@=       +@@@@@@@@@*  %@@@@@@@@@@   @@@@@@@@@@    
                                                                                                                             
                                                                                                                             
                                                                                                                             ");

app.Run();
