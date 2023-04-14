using PEngine.Common;
using PEngine.Common.DataModels;
using PEngine.Repositories;
using PEngine.States;

namespace PEngine.Services
{
    public class CommentService
    {
        private readonly UserContext _context;
        private readonly CommentRepository _repository;
        
        public CommentService(UserContext context, CommentRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<List<Comment>> FromPostId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentServiceResponse> Add(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentServiceResponse> Modify(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentServiceResponse> Delete(int id)
        {
            throw new NotImplementedException();
        }
        
    }
}
