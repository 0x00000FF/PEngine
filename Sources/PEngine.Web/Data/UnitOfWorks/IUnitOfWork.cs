using PEngine.Web.Data.Repositories;

namespace PEngine.Web.Data.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        public TRepository? Entity<TRepository>(string entityName) where TRepository : class, IRepository;
        public Task<int> CompleteAsync();
    }
}
