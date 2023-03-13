using System.ComponentModel.DataAnnotations;

namespace PEngine.Models.DataModels;

public class Attachment
{
    [Key] 
    public Guid Id { get; set; }

    public string Filename { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }

    public bool Streamable { get; set; }
}