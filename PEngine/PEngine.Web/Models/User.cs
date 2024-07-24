using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace PEngine.Web.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PasswordSalt { get; set; }
        public string? Name { get; set; }
        public UserRole Role { get; set; } = UserRole.Reader;
    }

    public enum UserRole
    {
        Root, Editor, Author, Contributor, Reader
    }
}
