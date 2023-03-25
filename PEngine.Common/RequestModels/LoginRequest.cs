namespace PEngine.Common.RequestModels
{
    public class LoginRequest : IPEngineModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public byte[] Nonce { get; set; } = Array.Empty<byte>();

        public ValidationState IsValid()
        {
            return ValidationState.Failed("test");
        }
    }
}
