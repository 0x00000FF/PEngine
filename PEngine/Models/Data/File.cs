namespace PEngine.Models.Data;

public class File
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Email { get; set; }
    
    public string Filetype { get; set; }

    public string? Service { get; set; }
    public string Path { get; set; }
}