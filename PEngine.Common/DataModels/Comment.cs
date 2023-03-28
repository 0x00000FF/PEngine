using System.ComponentModel.DataAnnotations;
using PEngine.Common.Misc;

namespace PEngine.Common.DataModels;

public class Comment : ControlledObject
{
    [Key]
    public int Id { get; set; }

    public Guid? Writer { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    
    [Required]
    public string Content { get; set; } = null!;

    public bool Encrypted { get; set; }
    public string? EncryptionSalt { get; set; }
    public string? EncryptionMethod { get; set; }

    public int? ReplyOf { get; set; }
}