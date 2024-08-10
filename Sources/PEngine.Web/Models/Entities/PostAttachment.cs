using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class PostAttachment
    {
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        public Guid FileId { get; set; }
        [ForeignKey("FileId")]
        public File File { get; set; }
    }
}
