using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registracija Blazor Server komponenti (Bez klijentskog projekta)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Registracija SQLite baze podataka
builder.Services.AddDbContextFactory<TodoList.TodoDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // LOKALNO: Isključujemo cache i prisiljavamo Windows da odmah urezuje zadnju stavku na disk
        string fiksnaLokalnaPutanja = @"C:\Users\Miljenko Temer\source\repos\TodoList\TodoList\todonova.db";
        options.UseSqlite($"Data Source={fiksnaLokalnaPutanja};Journal Mode=Delete;");
    }
    else
    {
        // NA WEB-U (RAILWAY): Rad u izoliranom Docker Linux okruženju
        string prodDbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todonova.db");
        options.UseSqlite($"Data Source={prodDbPath}");
    }
});

var app = builder.Build();

// 3. Konfiguracija HTTP cjevovoda (Bez redirectiona koji ruši produkciju)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

// 4. Mapiranje na glavnu App komponentu s punom stazom
app.MapRazorComponents<TodoList.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
