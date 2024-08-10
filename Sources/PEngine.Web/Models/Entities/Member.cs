using System;
using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models.Entities
{
    public class Member
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(128)]
        public string Username { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }

        [Required, StringLength(128)]
        public string Name { get; set; }

        [Required, StringLength(128)]
        public string Email { get; set; }

        [Required]
        public bool Disabled { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool UseIpSecurity { get; set; }

        public DateTime? PasswordExpires { get; set; }
        public DateTime? BlockedUntil { get; set; }
    }
}
