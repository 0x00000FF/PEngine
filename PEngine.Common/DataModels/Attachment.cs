using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using PEngine.Common.Misc;

namespace PEngine.Common.DataModels;

public class Attachment : ControlledObject
{
    [Key] 
    public Guid Id { get; set; }

    public AttachmentType Type { get; set; } = AttachmentType.OnPremise;
    public string StoragePath { get; set; } = "/default";

    public string Filename { get; set; } = "unknown";
    public string ContentType { get; set; } = MediaTypeNames.Application.Octet;
    public long Size { get; set; }

    public bool Streamable { get; set; }

    public Guid UploadedBy { get; set; }
}

public enum AttachmentType
{
    OnPremise, ObjectStorage, Ipfs
}