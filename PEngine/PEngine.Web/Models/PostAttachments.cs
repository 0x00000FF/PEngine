using Microsoft.EntityFrameworkCore;

namespace PEngine.Web.Models;

[Keyless]
public class PostAttachments
{
    public long PostId { get; set; }
    public Guid FileTagId { get; set; }
}