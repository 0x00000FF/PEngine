using System;
using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models.Entities
{
    public class AuthFactor
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(64)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        [Required]
        public string Endpoint { get; set; }

        public string Key { get; set; }

        public string Secret { get; set; }

        public string ExtraConfig { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
