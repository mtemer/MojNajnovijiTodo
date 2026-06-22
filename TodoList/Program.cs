using Microsoft.EntityFrameworkCore;
using TodoList;
using TodoList.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Registracija Blazor komponenti s izravnim podizanjem mrežnog limita na 100 MB (za teške PDF-ove)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 1024 * 1024 * 100; // Podignuto na 100 MB
    })
    .AddInteractiveWebAssemblyComponents();

// 2. Registracija SQLite baze podataka na trajnu Railway lokaciju (/data/)
builder.Services.AddDbContextFactory<TodoDbContext>(options =>
    options.UseSqlite("Data Source=/data/todo.db"));

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

// 3. Automatsko pokretanje migracija i kreiranje baze na Linuxu (Railway Volume)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

    // Osiguravamo da /data mapa postoji na Linuxu prije nego SQLite pokuša kreirati datoteku
    var dbFolder = Path.GetDirectoryName("/data/todo.db");
    if (!string.IsNullOrEmpty(dbFolder))
    {
        Directory.CreateDirectory(dbFolder);
    }

    // Automatski primjenjuje sve migracije na produkcijsku bazu
    dbContext.Database.Migrate();
}

app.Run();
