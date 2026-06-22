using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registracija Blazor Server komponenti s podrškom za velike PDF poruke (100MB)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<Microsoft.AspNetCore.SignalR.HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024 * 100; // 100 MB
});

// 2. Registracija SQLite baze podataka s punom, nepogrešivom stazom za kontekst
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

// Otvaranje i sigurno kreiranje baze pri pokretanju aplikacije na poslužitelju
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

// 4. Mapiranje komponenti s punom stazom
app.MapRazorComponents<TodoList.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
