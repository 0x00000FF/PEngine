using System.Net;
using Newtonsoft.Json;
using PEngine.Common.DataModels;
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

    public IPAddress AuthenticatedRemoteAddress { get; private set; } = null!;
    public IPAddress? RemoteAddress { get; private set; } = null!;
    public Guid RoleId { get; private set; }
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
        
        RemoteAddress = accessor.HttpContext.Connection.RemoteIpAddress;

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

        AuthenticatedRemoteAddress = RemoteAddress;
        Expires = DateTimeOffset.Now.AddMinutes(10);
        RoleId = user.Role;
        UserId = user.Id;
        ContextHmac = CalculateContextHmac().AsBase64();

        var json = JsonConvert.SerializeObject(this);
        Session.SetString(TOKEN_NAME, json);
    }

    public void ExitUserContext()
    {
        AuthenticatedRemoteAddress = IPAddress.None;

        Session.Clear();
        Cookies.Delete(TOKEN_COOKIE);
    }

    private void ExpandSessionToken(string? token)
    {
        var obj = JsonConvert.DeserializeObject<UserContext>(token ?? "");

        if (obj is null)
        {
            AuthenticatedRemoteAddress = IPAddress.None;
            RoleId = Guid.Empty;
            UserId = Guid.Empty;
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
        return AuthenticatedRemoteAddress.ToString()
            .AsBytes().Digest()
            .Digest(RoleId.ToString())
            .Digest(UserId.ToString())
            .Digest(BitConverter.GetBytes(Expires.UtcTicks))
            .Hmac("12345".AsBytes()); // TODO: replace hmac key with randomly-generated session key
    }

    private bool ContextValidInner()
    {
        if (RemoteAddress is null ||
            !RemoteAddress.Equals(AuthenticatedRemoteAddress) ||
            DateTimeOffset.Now > Expires)
        {
            return false;
        }

        RefreshContext();

        return CalculateContextHmac()
            .SequenceEqual(ContextHmac.AsBase64Bytes());
    }
}