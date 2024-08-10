using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class MemberWhitelist
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        [Required, StringLength(64)]
        public string Country { get; set; }

        [Required, StringLength(32)]
        public string IPAddress { get; set; }

        [StringLength(256)]
        public string UserAgent { get; set; } = "*";

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
