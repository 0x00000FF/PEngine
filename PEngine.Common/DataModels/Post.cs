using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PEngine.Common.Misc;

namespace PEngine.Common.DataModels;

public class Post : ControlledObject
{
    [Key] 
    public int Id { get; set; }

    [Required]
    public Guid Writer { get; set; }
    
    [Required]
    public string Title { get; set; } = null!;
    
    [Required]
    public string Content { get; set; } = null!;
    
    [Required]
    public string Category { get; set; } = null!;
    public string? Tag { get; set; }

    [NotMapped]
    public List<string> TagList
    {
        get => JsonConvert.DeserializeObject<List<string>>(Tag ?? "") ?? new List<string>();
        set => Tag = JsonConvert.SerializeObject(value);
    }
    
    public bool SystemPost { get; set; }
    public bool Hidden { get; set; }
    
    public bool Encrypted { get; set; }
    public string? EncryptionSalt { get; set; }
    public string? EncryptionMethod { get; set; }

    public uint ReadCount { get; set; }
}