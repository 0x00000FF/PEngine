using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class MemberAuthFactor
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        public Guid FactorId { get; set; }
        [ForeignKey("FactorId")]
        public AuthFactor Factor { get; set; }

        public string EncToken { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiredAt { get; set; }

        [Required, StringLength(32)]
        public string IPAddress { get; set; }
    }
}
