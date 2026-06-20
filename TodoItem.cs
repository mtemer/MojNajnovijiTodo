namespace TodoList;

public class TodoItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsDone { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Sifra { get; set; }
    public byte[]? PdfContent { get; set; }
    public string? PdfFileName { get; set; }
}
