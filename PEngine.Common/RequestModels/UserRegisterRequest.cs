namespace PEngine.Common.RequestModels;

public class UserRegisterRequest : IPEngineModel
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirm { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    
    public ValidationState IsValid()
    {
        throw new NotImplementedException();
    }
}