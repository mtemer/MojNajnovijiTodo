using Microsoft.EntityFrameworkCore;
using TodoList.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Dodavanje servisa kontejneru (Očišćeno od duplog koda)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// aplikacija i lokalno i na webu čita datoteku todo.db iz mape same aplikacije.
// 2. Registracija SQLite baze podataka
builder.Services.AddDbContextFactory<TodoDbContext>(options =>
{
    // Ako smo lokalno u Visual Studiju, čitamo bazu direktno iz korijena projekta
    if (builder.Environment.IsDevelopment())
    {
        // Vraća putanju do korijena projekta (gdje su Program.cs i vaš todo.db)
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        string dbPath = Path.Combine(projectRoot, "todo.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
    else
    {
        // Na Railwayu čitamo bazu iz mape gdje je aplikacija objavljena
        options.UseSqlite("Data Source=todo.db");
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TodoList.Client._Imports).Assembly);

app.Run();