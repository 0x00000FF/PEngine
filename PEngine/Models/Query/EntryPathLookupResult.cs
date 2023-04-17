using PEngine.Models.Data;

namespace PEngine.Models.Query;

public class EntryPathLookupResult
{
    public Guid Id { get; set; }
    public EntryType Type { get; set; }
    public EntryMode Perm { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    
    public Guid? CreatorId { get; set; }
    public string? CreatorName { get; set; }

    public Guid? OwnerId { get; set; }
    public string? OwnerName { get; set; }
}