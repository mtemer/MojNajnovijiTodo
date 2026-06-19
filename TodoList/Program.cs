using Microsoft.EntityFrameworkCore;
using TodoList.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Dodavanje servisa kontejneru (Očišćeno od duplog koda)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// aplikacija i lokalno i na webu čita datoteku todo.db iz mape same aplikacije.
// 2. Registracija SQLite baze podataka
// 2. Registracija SQLite baze podataka s preciznim putanjama
builder.Services.AddDbContextFactory<TodoDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Lokalno u Visual Studiju čitamo direktno iz korijena projekta
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        string dbPath = Path.Combine(projectRoot, "todo.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
    else
    {
        // NA RAILWAYU: Prisilno čitamo bazu iz mape gdje je aplikacija objavljena (/app/)
        string prodDbPath = Path.Combine(AppContext.BaseDirectory, "todo.db");
        options.UseSqlite($"Data Source={prodDbPath}");
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