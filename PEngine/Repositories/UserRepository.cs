using Microsoft.EntityFrameworkCore;
using PEngine.Common.DataModels;
using PEngine.Common.Utilities;
using PEngine.Utilities;

namespace PEngine.Repositories;

public class UserRepository : RepositoryBase
{
    private readonly DbSet<User> _users;

    public UserRepository()
    {
        _users = Database.Users ?? throw new NullReferenceException("User DbSet cannot be null");
    }

    public async Task<bool> CreateUser(User newUser)
    {
        _users.Add(newUser);
        return await Database.SaveChangesAsync() > 0;
    }
    
    public async Task<User?> FromUsernameAndPassword(string username, string password)
    {
        var preAuth = await _users.Select(u => new { u.Username, u.PasswordSalt })
                                                  .FirstOrDefaultAsync();

        if (preAuth is null)
        {
            return null;
        }
        
        var digest = Hash.MakePassword(password, preAuth.PasswordSalt);
        return await _users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == digest);
    }
}