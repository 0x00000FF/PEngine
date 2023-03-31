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
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Roles { get; set; }
    
    [NotMapped]
    public List<Guid>? RoleList
    {
        get
        {
            var list = JsonConvert.DeserializeObject<List<Guid>>(Roles ?? "");

            if (list is null)
            {
                RoleList = new List<Guid> { Id };
                Roles = JsonConvert.SerializeObject(RoleList);

                list = RoleList;
            }
            
            return list;
        }
        set => Roles = JsonConvert.SerializeObject(value);
    }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

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


    public string? SnsHandles { get; set; }
}