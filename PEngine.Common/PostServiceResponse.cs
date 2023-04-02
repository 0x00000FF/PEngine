using PEngine.Common.DataModels;

namespace PEngine.Common;

public sealed class PostServiceResponse : ServiceResponse<Post>
{
    public bool IsEncrypted => Payload?.Encrypted ?? false;
    public bool IsHidden => Payload?.Hidden ?? false;
    public bool IsSystem => Payload?.SystemPost ?? false;
}