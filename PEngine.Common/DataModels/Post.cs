using System.ComponentModel.DataAnnotations;

namespace PEngine.Common.DataModels;

public class Post
{
    [Key] 
    public int Id { get; set; }

    public Guid Writer { get; set; }
    
    public string Title { get; set; }
    public string Content { get; set; }
    public string Category { get; set; }
    public string Tag { get; set; }
    
    public bool SystemPost { get; set; }
    public bool Hidden { get; set; }
    
    public bool Encrypted { get; set; }
    public string? EncryptionSalt { get; set; }
    public string? EncryptionMethod { get; set; }

    public DateTimeOffset WrittenAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public uint ReadCount { get; set; }
}