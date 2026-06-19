using Microsoft.EntityFrameworkCore;
using TodoList.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Dodavanje servisa kontejneru (Očišćeno od duplog koda)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// aplikacija i lokalno i na webu čita datoteku todo.db iz mape same aplikacije.
builder.Services.AddDbContextFactory<TodoDbContext>(options =>
    options.UseSqlite("Data Source=todo.db"));

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