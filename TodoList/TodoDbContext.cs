using Microsoft.EntityFrameworkCore;

//upravlja vezom s bazom podataka
public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PRISILNO UPISIVANJE VAŠE JEDNE ISPRAVNE STAVKE KROZ KOD
        modelBuilder.Entity<TodoItem>().HasData(
            new TodoItem
            {
                Id = 1, // Id mora biti fiksni broj za seeding
                Title = "Upišite ovdje točan naziv vaše jedne stavke",
                Sifra = "Ovdje upišite šifru",
                IsDone = false,
                DueDate = null // ili stavite npr. new DateTime(2026, 6, 19) ako ima datum
            }
        );
    }
}