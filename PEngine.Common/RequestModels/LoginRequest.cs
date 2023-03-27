namespace PEngine.Common.RequestModels
{
    public class LoginRequest : IPEngineModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public byte[] Nonce { get; set; } = Array.Empty<byte>();

        public ValidationState IsValid()
        {
            if (string.IsNullOrEmpty(Username?.Trim()))
            {
                return ValidationState.Failed("Username is not valid.");
            }

            if (string.IsNullOrEmpty(Password?.Trim()))
            {
                return ValidationState.Failed("Password is not valid.");
            }

            return ValidationState.Success;
        }
    }
}
