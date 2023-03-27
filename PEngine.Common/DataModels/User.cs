using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PEngine.Common.DataModels;

[Index(nameof(Username), nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid Role { get; set; } = Guid.Empty;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string PasswordSalt { get; set; } = null!;

    public Guid? Profile { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    public string? Bio { get; set; }

    public string? SNSHandles { get; set; }
}