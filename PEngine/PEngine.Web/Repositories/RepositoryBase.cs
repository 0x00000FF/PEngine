namespace PEngine.Web.Repositories;

public abstract class RepositoryBase
{
    protected BlogContext _context;
    
    public RepositoryBase(BlogContext context)
    {
        _context = context;
    }
}