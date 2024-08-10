using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class Profile
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        public Guid VersionId { get; set; }
        [ForeignKey("VersionId")]
        public ProfileVersion Version { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
