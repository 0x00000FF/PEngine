using PEngine.Persistence;

namespace PEngine.Repositories
{
    public abstract class RepositoryBase
    {
        protected static readonly DatabaseContext Database = new DatabaseContext();
    }
}
