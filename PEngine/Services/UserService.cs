using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PEngine.Common.DataModels;
using PEngine.Common.RequestModels;
using PEngine.Repositories;

namespace PEngine.Services;

public class UserService
{
    private ISession? _session;
    private UserRepository _repository;
    
    public UserService(IHttpContextAccessor accessor, UserRepository repository)
    {
        _session = accessor.HttpContext?.Session;
        _repository = repository;
    }

    public async Task<User?> LoginAsync(LoginRequest request)
    {
        var user = await _repository.FromUsernameAndPassword(
            request.Username?.Trim() ?? string.Empty, 
            request.Password?.Trim() ?? string.Empty);

        if (user is not null)
        {
            
        }
        
        return user;
    }

    public async Task<bool> LoginPkiAsync(string token)
    {
        await Task.Run(() => throw new NotImplementedException());
        return false;
    }

    public async Task<bool> LoginFidoAsync(byte[] token)
    {
        await Task.Run(() => throw new NotImplementedException());
        return false;
    }
}