using Microsoft.EntityFrameworkCore;
using TodoList.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Dodavanje servisa kontejneru (Očišćeno od duplog koda)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// 2. Registracija SQLite baze podataka na novu trajnu Railway lokaciju (/app/data/)
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

// 3. Automatsko pokretanje migracija i kreiranje baze na Linuxu
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

    var dbFolder = Path.GetDirectoryName("/data/todo.db");
    if (!string.IsNullOrEmpty(dbFolder))
    {
        Directory.CreateDirectory(dbFolder);
    }

    // Zamijenjeno: EnsureCreated() -> Migrate()
    dbContext.Database.Migrate();
}

app.Run();