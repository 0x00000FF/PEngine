namespace PEngine.Models.Data;

public class User
{
    public Guid Id { get; set; }
    public Guid Group { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public Guid? ProfileImage { get; set; }
    public Guid? Signature { get; set; }
    
    public Guid Keyring { get; set; }
}