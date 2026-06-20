using TodoList.Components; // <-- DODAJTE OVAJ TOČAN RETAK NA SAMI VRH
using Microsoft.EntityFrameworkCore;
using TodoList;

var builder = WebApplication.CreateBuilder(args);

// 1. Dodajemo Blazor Server servise (Bez WebAssembly klijenta)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Registracija SQLite baze podataka
builder.Services.AddDbContextFactory<TodoDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        string fiksnaLokalnaPutanja = @"C:\Users\Miljenko Temer\source\repos\TodoList\TodoList\todonova.db";
        options.UseSqlite($"Data Source={fiksnaLokalnaPutanja}");
    }
    else
    {
        string prodDbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todonova.db");
        options.UseSqlite($"Data Source={prodDbPath}");
    }
});

var app = builder.Build();

// 3. Konfiguracija HTTP zahtjeva (Bez HttpsRedirection-a koji ruši Linux)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

// 4. Mapiranje komponenti isključivo za Server
app.MapRazorComponents<TodoList.Components.App>() // <-- ISPRAVLJENO S PUNOM STAZOM
    .AddInteractiveServerRenderMode();

app.Run();
