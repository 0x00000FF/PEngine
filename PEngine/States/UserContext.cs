using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using PEngine.Utilities;

namespace PEngine.States;

public class UserContext
{
    public const string TOKEN_NAME = "_pengine_context_token";
    
    public IPAddress AuthenticatedRemoteAddress { get; private set; } = null!;
    public IPAddress? RemoteAddress { get; private set; } = null!;
    public string RoleId { get; private set; } = null!;
    public string UserId { get; private set; } = null!;
    public string ContextHmac { get; private set; } = null!;
    public DateTimeOffset Expires { get; private set; }
    public bool ContextValid => ContextValidInner();

    public UserContext(IHttpContextAccessor accessor)
    {
        if (accessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }
        
        RemoteAddress = accessor.HttpContext.Connection.RemoteIpAddress;
        
        var session = accessor.HttpContext.Session;
        var token = session.GetString(TOKEN_NAME);
        
        ExpandSessionToken(token);
    }

    private void ExpandSessionToken(string? token)
    {
        var obj = JsonConvert.DeserializeObject<UserContext>(token ?? "");

        if (obj is null)
        {
            AuthenticatedRemoteAddress = IPAddress.None;
            RoleId = string.Empty;
            UserId = string.Empty;
            ContextHmac = string.Empty;
            Expires = DateTimeOffset.MinValue;

            return;
        }

        AuthenticatedRemoteAddress = obj.AuthenticatedRemoteAddress;
        RoleId = obj.RoleId;
        UserId = obj.UserId;
        ContextHmac = obj.ContextHmac;
        Expires = obj.Expires;
    }

    private bool ContextValidInner()
    {
        if (DateTimeOffset.Now < Expires ||
            RemoteAddress is null ||
            !RemoteAddress.Equals(AuthenticatedRemoteAddress))
        {
            return false;
        }

        return AuthenticatedRemoteAddress.ToString()
            .AsBytes().Digest().Digest(RoleId).Digest(UserId)
            .Digest(BitConverter.GetBytes(Expires.UtcTicks))
            .Hmac("12345".AsBytes())
            .SequenceEqual(ContextHmac.AsBase64Bytes());
    }
}