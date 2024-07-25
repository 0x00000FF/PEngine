using Npgsql;
using PEngine.Web.Data.Repositories;
using System.Data;

namespace PEngine.Web.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        IDbConnection _connection;
        IDbTransaction _transaction;

        public UnitOfWork(NpgsqlConnection connection)
        {
            _connection = connection;
            _transaction = _connection.BeginTransaction();
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                _transaction.Commit();
                return 1;
            }
            catch
            {
                return await RollbackAsync();
            }
        }

        public void Dispose()
        {
            
        }

        public TRepository? Entity<TRepository>(string entityName) where TRepository : class, IRepository
        {
            var thisType = this.GetType();
            var repoProp = thisType.GetProperty(entityName);

            return repoProp?.GetValue(this) as TRepository;
        }

        public async Task<int> RollbackAsync()
        {
            _transaction.Rollback();
            return 0;
        }
    }
}
