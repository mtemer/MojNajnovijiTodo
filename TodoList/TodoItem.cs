public class TodoItem
{
    // 1. DODAJTE OVAJ REDAK (EF Core će ovo automatski prepoznati kao Primarni Ključ)
    // Nakon dodavanja novog polja u shell pokrenuti naredbu: dotnet ef migrations add AddIdToTodoItem i dotnet ef database update
    public int Id { get; set; }
    public string? Sifra { get; set; }
    public string? Title { get; set; }
    public bool IsDone { get; set; }
    public DateTime? DueDate { get; set; }
    public byte[]? PdfContent { get; set; }
    public string? PdfFileName { get; set; }
}