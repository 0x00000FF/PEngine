using Microsoft.EntityFrameworkCore;
using PEngine.Common;
using PEngine.Common.DataModels;
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

        public async Task<PostServiceResponse> FromId(int id)
        {
            var post = await _postRepository.FromId(id);

            return new PostServiceResponse { Success = post is not null, Payload = post };
        }

        public async Task<List<PostListItem>> FetchLatest()
        {
            return await _postRepository.ListPredicate(null, 0, 30)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostListItem())
                .ToListAsync();
        }
    }
}
