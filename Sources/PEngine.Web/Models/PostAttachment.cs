using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models
{
    public class PostAttachment
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public Guid PostId { get; set; }
    }
}
