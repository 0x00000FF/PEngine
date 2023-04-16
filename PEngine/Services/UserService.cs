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

}