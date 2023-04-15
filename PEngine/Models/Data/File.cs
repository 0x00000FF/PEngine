namespace PEngine.Models.Data;

public class File
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Email { get; set; }
    
    public string Content { get; set; }

    public bool Encrypted { get; set; }
    public Guid? Keyring { get; set; }
}