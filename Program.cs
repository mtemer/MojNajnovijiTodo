using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registracija Blazor Server komponenti S PODRŠKOM ZA VELIKE PDF DOKUMENTE (100MB)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options => options.MaxReceivedMessageSize = 1024 * 1024 * 100);

// 2. Registracija SQLite baze podataka
builder.Services.AddDbContextFactory<TodoList.TodoDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        string fiksnaLokalnaPutanja = @"C:\Users\Miljenko Temer\source\repos\TodoList\TodoList\todonova.db";
        options.UseSqlite($"Data Source={fiksnaLokalnaPutanja};");
    }
    else
    {
        string prodDbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todonova.db");
        options.UseSqlite($"Data Source={prodDbPath}");
    }
});

var app = builder.Build();

// Otvaranje i kreiranje baze pri pokretanju
using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TodoList.TodoDbContext>>();
    using var context = dbFactory.CreateDbContext();
    await context.Database.EnsureCreatedAsync();

    if (app.Environment.IsDevelopment())
    {
        await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=DELETE;");
    }
}

// 3. Konfiguracija HTTP cjevovoda
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

// 4. Mapiranje komponenti
app.MapRazorComponents<TodoList.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
