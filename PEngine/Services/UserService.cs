using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PEngine.Common;
using PEngine.Common.DataModels;
using PEngine.Common.RequestModels;
using PEngine.Repositories;
using PEngine.States;
using PEngine.Utilities;

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

    public User? GetCurrentUser()
    {
        return _context.ContextValid ? _repository.FromId(_context.UserId) : null;
    }

    public async Task RequestContextStart(User user)
    {
        await _context.BeginUserContext(user);
    }

    public async Task RequestContextExit()
    {
        if (_context.ContextValid)
            await _context.ExitUserContext();
    }
    
    public async Task<ServiceResponse<Guid?>> RegisterAsync(UserRegisterRequest request)
    {
        var newUser = new User
        {
            Username = request.Username!,
            Password = request.Password!,
            Name = request.Name!,
            Email = request.Email!
        };

        var registerResult = await _repository.CreateUser(newUser);

        return new ServiceResponse<Guid?>()
        {
            Success = registerResult,
            Payload = registerResult ? newUser.Id : null
        };
    }

    public async Task<ServiceResponse<User?>> LoginAsync(LoginRequest request)
    {
        var user = await _repository.FromUsernameAndPassword(
            request.Username?.Trim() ?? string.Empty,
            request.Password?.Trim() ?? string.Empty);

        if (user is not null)
        {
            await RequestContextStart(user);
        }

        return new ServiceResponse<User?>() { Success = user is not null, Payload = user };
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