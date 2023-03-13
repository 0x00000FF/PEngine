using System.ComponentModel.DataAnnotations;

namespace PEngine.Models.DataModels;

public class Post
{
    [Key] 
    public int Id { get; set; }

    public Guid Writer { get; set; }
    
    public string Title { get; set; }
    public string Content { get; set; }
    public string Path { get; set; }
    public string Tag { get; set; }
    
    public bool SystemPost { get; set; }
    public bool Hidden { get; set; }
    
    public bool Encrypted { get; set; }
    public string? EncryptionSalt { get; set; }
    public string? EncryptionMethod { get; set; }

    public DateTimeOffset WrittenAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    
    public int ReadCount { get; set; }
    
}