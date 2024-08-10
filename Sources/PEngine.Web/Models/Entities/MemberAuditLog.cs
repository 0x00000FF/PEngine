using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class MemberAuditLog
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        public Guid? InvokerId { get; set; }
        [ForeignKey("InvokerId")]
        public Member Invoker { get; set; }

        [Required, StringLength(32)]
        public string Action { get; set; }

        [Required, StringLength(256)]
        public string Description { get; set; }

        [Required, StringLength(32)]
        public string IPAddress { get; set; }

        [Required, StringLength(256)]
        public string UserAgent { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
