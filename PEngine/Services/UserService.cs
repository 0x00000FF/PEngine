using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PEngine.Common;
using PEngine.Common.DataModels;
using PEngine.Common.RequestModels;
using PEngine.Repositories;
using PEngine.States;

namespace PEngine.Services;

public class UserService
{
    private readonly UserContext _context;
    private readonly UserRepository _repository;
    
    public UserService(UserContext context, UserRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    public async Task<ServiceResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _repository.FromUsernameAndPassword(
            request.Username?.Trim() ?? string.Empty, 
            request.Password?.Trim() ?? string.Empty);

        if (user is not null)
        {
            
        }
        
        return new ServiceResponse() { Success = user is not null, Payload = user };
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