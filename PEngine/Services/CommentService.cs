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
        
    }
}
