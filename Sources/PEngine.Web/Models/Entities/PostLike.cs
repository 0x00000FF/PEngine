using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class PostLike
    {
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        public Guid? LoggedMemberId { get; set; }
        [ForeignKey("LoggedMemberId")]
        public Member LoggedMember { get; set; }

        [Required, StringLength(32)]
        public string IPAddress { get; set; }

        [Required, StringLength(256)]
        public string UserAgent { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
