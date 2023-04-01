using System.Net;
using Newtonsoft.Json;
using PEngine.Common.DataModels;
using PEngine.Common.Utilities;
using PEngine.Utilities;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace PEngine.States;

public class UserContext
{
    public static readonly string TOKEN_NAME = "_pengine_context_token";
    public static readonly string TOKEN_COOKIE = "_PENGINE_SESS";
    
    private ISession Session { get; }
    
    public IResponseCookies Cookies { get; }

    public byte[] AuthenticatedRemoteAddress
    {
        get => Session.Get(nameof(AuthenticatedRemoteAddress)) ?? Array.Empty<byte>();
        private set => Session.Set(nameof(AuthenticatedRemoteAddress), value);
    }

    public byte[]? RemoteAddress
    {
        get => Session.Get(nameof(AuthenticatedRemoteAddress)) ?? Array.Empty<byte>();
        private set => Session.Set(nameof(RemoteAddress), value!);
    }
    
    public List<Guid>? RoleList
    {
        get => JsonConvert.DeserializeObject<List<Guid>>(Session.GetString(nameof(RoleList)) ?? ""); 
        private set => Session.SetString(nameof(RoleList), JsonConvert.SerializeObject(value));
    }
    
    public Guid UserId
    {
        get => new(Session.GetString(nameof(UserId))!);
        private set => Session.SetString(nameof(UserId), value.ToString()); 
    }
    
    public string ContextHmac
    {
        get => Session.GetString(nameof(ContextHmac)) ?? "";
        private set => Session.SetString(nameof(ContextHmac), value); 
    }
    
    public DateTimeOffset Expires
    {
        get => DateTimeOffset.Parse(Session.GetString(nameof(Expires)) ?? "1970-01-01 00:00:00");
        private set => Session.SetString(nameof(Expires), value.ToString());
    }
    
    public bool ContextValid => ContextValidInner();

    public UserContext(IHttpContextAccessor accessor)
    {
        if (accessor?.HttpContext is null)
        {
            throw new InvalidOperationException();
        }
        
        Session = accessor.HttpContext.Session;
        Cookies = accessor.HttpContext.Response.Cookies;
        
        RemoteAddress = accessor.HttpContext.Connection.RemoteIpAddress?.GetAddressBytes();
    }

    public void BeginUserContext(User user)
    {
        if (RemoteAddress is null)
        {
            throw new InvalidDataException("User Context cannot be started when RemoteAddress is null.");
        }

        AuthenticatedRemoteAddress = RemoteAddress;
        Expires = DateTimeOffset.Now.AddHours(1);
        RoleList = user.RoleList ?? new List<Guid>() { user.Id };
        UserId = user.Id;
        ContextHmac = CalculateContextHmac().AsBase64();
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