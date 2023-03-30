using System.Net;
using Newtonsoft.Json;
using PEngine.Common.DataModels;
using PEngine.Common.Utilities;
using PEngine.Utilities;

namespace PEngine.States;

public class UserContext
{
    [JsonIgnore]
    public static readonly string TOKEN_NAME = "_pengine_context_token";

    [JsonIgnore]
    public static readonly string TOKEN_COOKIE = "_PENGINE_SESS";

    [JsonIgnore]
    private ISession Session { get; }

    [JsonIgnore]
    public IResponseCookies Cookies { get; }

    public byte[] AuthenticatedRemoteAddress { get; private set; } = null!;
    public byte[]? RemoteAddress { get; private set; }
    public List<Guid>? RoleList { get; private set; }
    public Guid UserId { get; private set; }
    public string ContextHmac { get; private set; } = null!;
    public DateTimeOffset Expires { get; private set; }

    [JsonIgnore]
    public bool ContextValid => ContextValidInner();

    public UserContext(IHttpContextAccessor accessor)
    {
        if (accessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }

        RemoteAddress = accessor.HttpContext.Connection.RemoteIpAddress?.GetAddressBytes();

        Cookies = accessor.HttpContext.Response.Cookies;
        Session = accessor.HttpContext.Session;

        var token = Session.GetString(TOKEN_NAME);

        ExpandSessionToken(token);
    }

    public void BeginUserContext(User user)
    {
        if (RemoteAddress is null)
        {
            throw new InvalidDataException("User Context cannot be started when RemoteAddress is null.");
        }

        AuthenticatedRemoteAddress = RemoteAddress ?? Array.Empty<byte>();
        Expires = DateTimeOffset.Now.AddMinutes(10);
        RoleList = user.RoleList ?? new List<Guid>() { user.Id };
        UserId = user.Id;
        ContextHmac = CalculateContextHmac().AsBase64();

        var json = JsonConvert.SerializeObject(this);
        Session.SetString(TOKEN_NAME, json);
    }

    public void ExitUserContext()
    {
        AuthenticatedRemoteAddress = Array.Empty<byte>();

        Session.Clear();
        Cookies.Delete(TOKEN_COOKIE);
    }

    private void ExpandSessionToken(string? token)
    {
        var obj = JsonConvert.DeserializeObject<UserContext>(token ?? "");

        if (obj is null)
        {
            AuthenticatedRemoteAddress = Array.Empty<byte>();
            UserId = Guid.Empty;
            ContextHmac = string.Empty;
            Expires = DateTimeOffset.MinValue;

            return;
        }

        AuthenticatedRemoteAddress = obj.AuthenticatedRemoteAddress;
        RoleList = obj.RoleList;
        UserId = obj.UserId;
        ContextHmac = obj.ContextHmac;
        Expires = obj.Expires;
    }

    private void RefreshContext()
    {
        if (DateTimeOffset.Now > Expires.AddMinutes(-1))
        {
            Expires = Expires.AddMinutes(10);
            ContextHmac = CalculateContextHmac().AsBase64();
        }
    }

    private byte[] CalculateContextHmac()
    {
        return AuthenticatedRemoteAddress.AsBase64()
            .AsBytes().Digest()
            .Digest(RoleList?.Select(g => g.ToString())
                             .Aggregate( (s1, s2) => s1.AsBytes().Digest(s2).AsBase64() )
                    ?? "")
            .Digest(UserId.ToString())
            .Digest(BitConverter.GetBytes(Expires.UtcTicks))
            .Hmac("12345".AsBytes()); // TODO: replace hmac key with randomly-generated session key
    }

    private bool ContextValidInner()
    {
        if (RemoteAddress is null ||
            !RemoteAddress.SequenceEqual(AuthenticatedRemoteAddress) ||
            DateTimeOffset.Now > Expires)
        {
            return false;
        }

        RefreshContext();

        return CalculateContextHmac()
            .SequenceEqual(ContextHmac.AsBase64Bytes());
    }
}