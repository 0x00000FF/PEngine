using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PEngine.Common.Utilities;

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
    private string PasswordHash { get; set; } = null!;

    [NotMapped]
    public string Password
    {
        get => PasswordHash;
        set
        {
            PasswordSalt = Cryptography.Random(32).AsBase64();
            PasswordHash = Hash.MakePassword(value, PasswordSalt);
        }
    }

    [Required]
    public string PasswordSalt { get; set; } = null!;

    public Guid? Profile { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    public string? Bio { get; set; }

    private string Roles { get; set; } = null!;

    [NotMapped]
    public List<Guid>? RoleList
    {
        get => JsonConvert.DeserializeObject<List<Guid>>(Roles) ?? new List<Guid>{};
        set => Roles = JsonConvert.SerializeObject(value);
    }

    public string? SnsHandles { get; set; }
}