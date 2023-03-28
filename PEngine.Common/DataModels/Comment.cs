using System.ComponentModel.DataAnnotations;
using PEngine.Common.Misc;

namespace PEngine.Common.DataModels;

public class Comment : ControlledObject
{
    [Key]
    public int Id { get; set; }

    public Guid? Writer { get; set; }

    public string Name { get; set; }
    public string? Email { get; set; }
    
    public string Content { get; set; }

    public bool Encrypted { get; set; }
    public string? EncryptionSalt { get; set; }
    public string? EncryptionMethod { get; set; }

    public int? ReplyOf { get; set; }
    public DateTimeOffset WrittenAt { get; set; } = DateTimeOffset.Now;
}