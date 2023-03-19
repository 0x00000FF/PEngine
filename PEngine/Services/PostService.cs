using PEngine.Repositories;

namespace PEngine.Services
{
    public class PostService
    {
        private RepositoryBase Repository;
        
        public PostService()
        {
            Repository = new PostRepository();
        }
    }
}
