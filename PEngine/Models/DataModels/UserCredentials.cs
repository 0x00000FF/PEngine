namespace PEngine.Models.DataModels;

public class UserCredentials
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid CredentialFor { get; set; }
    public UserCredentialType Type { get; set; }
    public string? Data { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? ExpiresAt { get; set; }
}

public enum UserCredentialType
{
    Certificate, Fingerprint, ExternalService
}