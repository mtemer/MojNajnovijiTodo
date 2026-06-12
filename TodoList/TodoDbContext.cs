using Microsoft.EntityFrameworkCore;

//upravlja vezom s bazom podataka
public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

    public DbSet<TodoItem> Todos { get; set; }
}