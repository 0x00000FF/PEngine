using Microsoft.EntityFrameworkCore;
using PEngine.ComponentModels;
using PEngine.Models.Data;
using PEngine.Models.Response;
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

        public async Task<IResponse> FetchDirectory(string path)
        {
            if (!path.StartsWith("/"))
            {
                return new EntryResponse
                {
                    Status = false,
                    Message = "Not Valid Path",
                    Id = null,
                    Type = null,
                    Name = null
                };
            }
            
            var list = 
        }
    }
}
