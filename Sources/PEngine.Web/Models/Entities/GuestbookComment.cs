using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class GuestbookComment
    {
        [Key]
        public int Id { get; set; }

        public int GuestbookId { get; set; }
        [ForeignKey("GuestbookId")]
        public Guestbook Guestbook { get; set; }

        public Guid VersionId { get; set; }
        [ForeignKey("VersionId")]
        public GuestbookCommentVersion Version { get; set; }

        [StringLength(32)]
        public string Writer { get; set; }

        public string Password { get; set; }
        public string Contact { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public bool IsBlocked { get; set; } = false;

        public string BlockReason { get; set; }
        public DateTime? BlockedAt { get; set; }
    }
}
