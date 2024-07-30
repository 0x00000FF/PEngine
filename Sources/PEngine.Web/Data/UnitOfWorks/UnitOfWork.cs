using Npgsql;
using PEngine.Web.Data.Repositories;
using System.Data;

namespace PEngine.Web.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        IDbConnection? _connection;
        IDbTransaction? _transaction;

        bool _completed;

        public UnitOfWork(NpgsqlConnection connection)
        {
            _connection = connection;
            _transaction = _connection?.BeginTransaction();
            _completed = false;
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                _transaction!.Commit();
                return 1;
            }
            catch
            {
                _transaction.Rollback();
                return 0;
            }
            finally
            {
                _completed = true;
            }
        }

        public void Dispose()
        { 
            if (_transaction is not null)
            {
                if (!_completed)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                }

                _transaction = null;
            }

            if (_connection is not null)
            {
                _connection = null;
            }
        }

        public TRepository? Entity<TRepository>(string entityName) where TRepository : class, IRepository
        {
            var thisType = this.GetType();
            var repoProp = thisType.GetProperty(entityName);

            return repoProp?.GetValue(this) as TRepository;
        }
    }
}
