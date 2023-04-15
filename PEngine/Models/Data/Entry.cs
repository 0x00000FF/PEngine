namespace PEngine.Models.Data;

public class Entry
{
    public Guid Id { get; set; }
    public Guid? Parent { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public EntryMode Mode { get; set; }
    public Guid Owner { get; set; }
    public Guid Group { get; set; }

    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    public DateTimeOffset Deleted { get; set; }

    public bool Hidden { get; set; }
    public bool Sealed { get; set; }

    public int Hits { get; set; }
    public string IpAddress { get; set; } = string.Empty;

    public Guid? Entity { get; set; }
}