using PEngine.Repositories;

namespace PEngine.Services
{
    public class PostService
    {
        private readonly RepositoryBase Repository;
        
        public PostService()
        {
            Repository = new PostRepository();
        }
    }
}
