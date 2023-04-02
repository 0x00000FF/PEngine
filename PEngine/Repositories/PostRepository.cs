using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PEngine.Common.DataModels;
using PEngine.ComponentModels;

namespace PEngine.Repositories
{
    public class PostRepository : RepositoryBase
    {
        public const int FETCH_SIZE = 30;
        
        private DbSet<Post> _posts;
            
        public PostRepository()
        {
            _posts = Database.Posts ?? throw new NullReferenceException("Post DbSet cannot be null");
        }

        public async Task<Post?> FromId(int id)
        {
            return await _posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Post> ListPredicate(Expression<Func<Post, bool>>? predicate, int offset, int amount)
        {
            return ListPredicate(predicate).Skip(offset).Take(amount);
        }
        
        public IQueryable<Post> ListPredicate(Expression<Func<Post, bool>>? predicate, int offset)
        {
            return ListPredicate(predicate, offset,  -1);
        }
        
        public IQueryable<Post> ListPredicate(Expression<Func<Post, bool>>? predicate)
        {
            var query = _posts.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }
        
        public async Task<List<PostListItem>> GetLatestListings(int offset)
        {
            int skip = offset;
            var query = _posts.OrderByDescending(p => p.CreatedAt);
            
            var latestPosts = await query
                .Select(p => new PostListItem
                {
                    Id = p.Id,
                    Title = p.Title,
                    Category = p.Category,
                    DateCreated = p.CreatedAt,
                    DateModified = p.ModifiedAt ?? p.CreatedAt,
                    Author = "",
                    Size = p.Content.Length.ToString("D Bytes"),
                })
                .Skip(skip)
                .Take(FETCH_SIZE).ToListAsync();

            return latestPosts;
        }
        
        
    }
}
