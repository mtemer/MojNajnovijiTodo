using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registracija Blazor Server komponenti
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Registracija SQLite baze podataka s čistim connection stringom
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

// KONAČNI POPRAVAK ZA PRISILNO URADNJU NA DISK LOKALNO
using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TodoList.TodoDbContext>>();
    using var context = dbFactory.CreateDbContext();
    
    // Prisno kreiramo bazu ako ne postoji
    await context.Database.EnsureCreatedAsync();

    // Ako radimo lokalno na računalu, prisilno gasimo privremeni journal cache
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
