namespace TodoList;

public class TodoItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Sifra { get; set; }
    public bool IsDone { get; set; }
    public DateTime? DueDate { get; set; }
    public string? PdfFileName { get; set; }

    // Izmijenite ovo polje da bude byte[] umjesto string
    public byte[]? PdfContent { get; set; }
}
