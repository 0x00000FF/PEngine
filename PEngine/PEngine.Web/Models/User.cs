using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PEngine.Web.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PasswordSalt { get; set; }
        public string? Name { get; set; }
    }
}
