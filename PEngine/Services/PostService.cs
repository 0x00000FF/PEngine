using Microsoft.EntityFrameworkCore;
using PEngine.ComponentModels;
using PEngine.Repositories;
using PEngine.States;

namespace PEngine.Services
{
    public class PostService
    {
        private readonly PostRepository _postRepository;
        private readonly UserService _userService;

        public PostService(PostRepository postRepository, UserService userService)
        {
            _postRepository = postRepository;
            _userService = userService;
        }

    }
}
