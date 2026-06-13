public class TodoItem
{
    // 1. DODAJTE OVAJ REDAK (EF Core će ovo automatski prepoznati kao Primarni Ključ)
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsDone { get; set; }
    public DateTime? DueDate { get; set; }
}