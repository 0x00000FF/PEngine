using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class MemberRole
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int Ord { get; set; }
    }
}
